using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;

using BigInteger = System.Numerics.BigInteger;
using Suinet.Wallet;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using SUI.BCS;
using Chaos.NaCl;
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Numerics;

namespace SH
{
    public class SuiWalletManager : MonoBehaviour
    {
        private static string _clockAddress = "0x0000000000000000000000000000000000000000000000000000000000000006";

        private static string _packageAddress = "0x6c770a38a07c937998bb0249e70101e79eda3848aea907cb90e56fad6fe62d8a";
        private static string _farmerDataAddress = "0x2c62f0910528e667e39271de742acdf2f0df8be2da03f3736f84b6bb822e3cf6";
        private static string _minterDataAddress = "0x1a5fb6ddf4ef2b207876fa1d6a88bce0a9946592757d40ac70668dfc90453dfe";
        private static string _craftingDataAddress  = "0xc95ea85cd1f8dc4b87c0dcb7bb2b7614e246f161acc760ffec1e923b357b66fa";

        private static string _hunterAddress {get; set;}
        private static string _hunterSymbol = "HTR1";
        private static string _swordSymbol = "SWD1";

        //character
        private static string _nftCharacterPackageId = "0x201e77838a6f75d1e6b6808052d0049bb38e880fba41fd7b6d2cde99150edd6a";
        private static string _nftCharacterId = "0x05843aeab83f3f144d138d3294eec3037dad24e58e05342714bed90b4db59bed";

        //mineral
        private static string _nftMineralPackageId = "0x50d80703f4f3ad80284d999a49f6978ed52dfa37965ec9970daf8e541de1b3bc";
        private static string _nftMineralId = "0x38234dbc1abc10f24394d289e9f9b69b6a2839db574227cd05c97a05302c7726";


        public static async Task<string> GetSuiWalletBalance()
        {
            string coinType = "0x2::sui::SUI";

            var balanceResult = await SuiApi.Client.GetBalanceAsync(SuiWallet.GetActiveAddress(), coinType);

            float balance = ((float)balanceResult.Result.TotalBalance) / 1000000000;

            return balance.ToString("0.#########");
        }


        public async static Task<RpcResult<TransactionBlockResponse>> Execute(RpcResult<TransactionBlockBytes> result)
        {
            var rpcResult = new RpcResult<TransactionBlockResponse>();

            if (result.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();

                var txBytes = result.Result.TxBytes;
                var rawSigner = new RawSigner(keyPair);
                
                var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));

                rpcResult = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] { signature.Value }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);
            }
            else
            {
                Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
            }

            Debug.Log(rpcResult.RawRpcResponse);

            return rpcResult;
        }

        public async static Task<RpcResult<TransactionBlockBytes>> MintHunter()
        {

            var signer = SuiWallet.GetActiveAddress();
            var module = "mint";
            var function = "mint_hunter";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _minterDataAddress,
                _hunterSymbol,
            };
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

            return rpcResult;
        }

        public async static Task<RpcResult<TransactionBlockBytes>> StartFarming()
        {

            var signer = SuiWallet.GetActiveAddress();
            var module = "farming";
            var function = "start_farming";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _farmerDataAddress,
                InventoryManager.Instance.CurrentHunterAddressInUse,
                _clockAddress
            };
           
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);
            Debug.Log(rpcResult.RawRpcResponse);

            return rpcResult;
        }

        public async static Task<RpcResult<TransactionBlockBytes>> EndFarming(ulong exp, List<ulong> amountStone, List<string> symbolStone)
        {

            var signer = SuiWallet.GetActiveAddress();
            var module = "farming";
            var function = "end_farming";
            var typeArgs = System.Array.Empty<string>();

            BcsEncoder encoder = new BcsEncoder();

            encoder.RegisterType<string>("string",
                (writer, data, options, parameters) =>
                {
                    writer.WriteString((string)data);
                    return null;
                },
                null
            );

            // Register the u64 type
            encoder.RegisterType<ulong>("u64",
                (writer, data, options, parameters) =>
                {
                    writer.WriteUInt64((ulong)data);
                    return null;
                },
                null
            );

            encoder.RegisterType<List<byte>>("vector<u8>",
                 (writer, data, options, parameters) =>
                    {
                        List<byte> vector = (List<byte>)data;
                        writer.WriteVec(vector, (w, item, index, count) =>
                           {
                               w.WriteByte(item);
                           });
                        return null; // Return null as the result
                    },
            null // No validation callback specified
            );
            
            encoder.RegisterType<List<string>>("vector<string>", (writer, data, options, parameters) =>
            {
                List<string> vector = (List<string>)data;
                writer.WriteULEB((ulong)vector.Count);
                    foreach (string element in vector)
                     {
                        writer.WriteString(element);
                     }
                    return null;
            });

          

            List<byte> amountByteList = amountStone.Select(ulongValue => (byte)ulongValue).ToList();

            byte[] expByte = encoder.Serialize("u64", exp);

            byte[] amountByte = encoder.Serialize("vector<u8>", amountByteList);
             
            byte[] addressByte = encoder.Serialize("string", SuiWallet.GetActiveAddress().Substring(2));

            Debug.Log( "active add " + SuiWallet.GetActiveAddress());
            
            byte[] symbolByte = encoder.Serialize("vector<string>", symbolStone);

            int random = UnityEngine.Random.Range(0, 100000);
            ulong nonceEndFarming =  Convert.ToUInt64(random);

            byte[] nonceByte = encoder.Serialize("u64",nonceEndFarming);
          
            byte[] combinedBytes = expByte.Concat(amountByte).Concat(symbolByte).Concat(addressByte).Concat(nonceByte).ToArray();

            var bcsSignature = new List<byte>(SuiWallet.GetActiveKeyPair().Sign(combinedBytes));

            Debug.Log(bcsSignature);

            var exchanger = new List<byte>(SuiWallet.GetActiveKeyPair().PublicKey);
           

            var args = new object[] {
                _farmerDataAddress,
                _minterDataAddress,
                InventoryManager.Instance.CurrentHunterAddressInUse,
                _clockAddress,
                bcsSignature,
                exchanger,
                exp,
                amountStone,
                symbolStone,
                nonceEndFarming
            };
            var gasBudget = BigInteger.Parse("100000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

            Debug.Log(rpcResult.RawRpcRequest);

            return rpcResult;
        }

        public static void TestSignature() {
            
            BcsEncoder encoder = new BcsEncoder();

            encoder.RegisterType<string>("string",
                (writer, data, options, parameters) =>
                {
                    writer.WriteString((string)data);
                    return null;
                },
                null
            );

            // Register the u64 type
            encoder.RegisterType<ulong>("u64",
                (writer, data, options, parameters) =>
                {
                    writer.WriteUInt64((ulong)data);
                    return null;
                },
                null
            );

            encoder.RegisterType<List<byte>>("vector<u8>",
                 (writer, data, options, parameters) =>
                    {
                        List<byte> vector = (List<byte>)data;
                        writer.WriteVec(vector, (w, item, index, count) =>
                           {
                               w.WriteByte(item);
                           });
                        return null; // Return null as the result
                    },
            null // No validation callback specified
            );
            
            encoder.RegisterType<List<string>>("vector<string>", (writer, data, options, parameters) =>
            {
                List<string> vector = (List<string>)data;
                writer.WriteULEB((ulong)vector.Count);
                    foreach (string element in vector)
                     {
                        writer.WriteString(element);
                     }
                    return null;
            });

          

            List<byte> amountByteList = new List<byte>{1};
            ulong exptest= 100;
            byte[] expByte = encoder.Serialize("u64", exptest);

            byte[] amountByte = encoder.Serialize("vector<u8>", amountByteList);
             
            byte[] addressByte = encoder.Serialize("string", SuiWallet.GetActiveAddress());
            
            byte[] symbolByte = encoder.Serialize("vector<string>", new List<string> {"dst_stone"});

            int random = UnityEngine.Random.Range(0, 100000);
            ulong nonceEndFarming =  Convert.ToUInt64(10);

            byte[] nonceByte = encoder.Serialize("u64",nonceEndFarming);
          
            byte[] combinedBytes = expByte.Concat(amountByte).Concat(symbolByte).Concat(addressByte).Concat(nonceByte).ToArray();

            var bcsSignature = new List<byte>(SuiWallet.GetActiveKeyPair().Sign(combinedBytes));

            Debug.Log(bcsSignature);

            foreach(byte b in bcsSignature) {
                Debug.Log(b);
            }
        }

        public static async Task<RpcResult<TransactionBlockBytes>> CraftSword(List<string> stoneList) {
             var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "crafting_sword";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _craftingDataAddress,
                _minterDataAddress,
                stoneList,
                _swordSymbol
            };
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

            return rpcResult;
        }
        
        public async static Task<RpcResult<Page_for_DynamicFieldInfo_and_ObjectID>> GetHunterWeaponEquipment()
        {
            string hunterAddress = InventoryManager.Instance.CurrentHunterAddressInUse;

            var allEquipment = await SuiApi.Client.GetDynamicFieldsAsync(hunterAddress,null, null);

            return allEquipment;
        }

        public async static Task<RpcResult<SuiObjectResponse>> GetWeaponInfo(string address)
        {       
            return await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
        }

        public async static Task<RpcResult<SuiObjectResponse>> GetHunterInfo(string address)
        {       
            return await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
        }
        




    

        public async static Task<RpcResult<TransactionBlockBytes>> EquipWeapon(string address) {
           
            var weaponObjectResult = await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
            
            MoveObjectData weaponObjectData = weaponObjectResult.Result.Data.Content as MoveObjectData;

            var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "equip";
            var typeArgs = new List<string>{weaponObjectData.Type};
            var args = new object[] {
                InventoryManager.Instance.CurrentHunterAddressInUse,
                address,
                "sword"
            };
           
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);
            Debug.Log(rpcResult.RawRpcResponse);

            return rpcResult;
        }

        public async static Task<RpcResult<TransactionBlockBytes>> UnEquipWeapon(string address) {
           
            var weaponObjectResult = await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
            Debug.Log(weaponObjectResult.RawRpcResponse);
            MoveObjectData weaponObjectData = weaponObjectResult.Result.Data.Content as MoveObjectData;

            var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "cancel_equip";
            var typeArgs = new List<string>{weaponObjectData.Type};
            var args = new object[] {
                InventoryManager.Instance.CurrentHunterAddressInUse,
                "sword"
            };
           
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);
            Debug.Log(rpcResult.RawRpcResponse);

            return rpcResult;
        }



        public static async Task<RpcResult<TransactionBlockResponse>> MintMineral()
        {

            ulong amount = 1;

            var mintRpcResult = new RpcResult<TransactionBlockResponse>();

            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = _nftMineralPackageId;
            var module = "stone";
            var function = "claim_stone";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { _nftMineralId, amount };
            var gasBudget = BigInteger.Parse("1000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasBudget);
            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();


                var txBytes = rpcResult.Result.TxBytes;
                var rawSigner = new RawSigner(keyPair);
                var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));

                mintRpcResult = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] { signature.Value }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);
            }
            else
            {
                Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
            }

            return mintRpcResult;
        }
        
  
        public static async Task<RpcResult<Page_for_SuiObjectResponse_and_ObjectID>> GetAllNFT()
        {
            ObjectDataOptions optionsReq = new ObjectDataOptions();

            optionsReq.ShowContent = true;
            var allObject = await SuiApi.Client.GetOwnedObjectsAsync(SuiWallet.GetActiveAddress(),
                new ObjectResponseQuery() { Options = optionsReq }, null, null);
            Debug.Log(allObject.RawRpcResponse);
            return allObject;
        }


    }

}
