using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Suinet.NftProtocol;
using Suinet.Rpc;
using Suinet.Rpc.Client;
using Suinet.Rpc.Signer;
using Suinet.Wallet;

using BigInteger = System.Numerics.BigInteger;
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

        private static string _packageAddress = "0xb1eedbc4b571ce2e472cd910ee560f4f6c1d8eb9a3756225874f4f577e8d814c";
        private static string _farmerDataAddress = "0xb2b02c51794af3250094b527047b86d8ad0df9b889cff4af8e336d0941dea148";
        private static string _minterDataAddress = "0x17e3a806d528b26a9aced0089ff152cf5039bac2d6a398428c7f336bf4914cb2";
        private static string _craftingDataAddress  = "0x236f927b0361f6b9fa46624fb203072872ba5963f01df69d02a324242043bcd4";

        private static string _hunterAddress {get; set;}
        private static string _hunterSymbol = "HTR1";
        private static string _swordSymbol = "SWD1";
        private static string _glassSymbol = "GL1";

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
            UIManager.Instance.ShowWaiting();
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
            UIManager.Instance.HideWaiting();

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

        public async static Task<RpcResult<TransactionBlockResponse>> Add_Exchanger() {
            
            IKeyPair adminKey = Mnemonics.GetKeypairFromMnemonic("powder flock dog shrimp wage ordinary bless minimum calm raise visit rude");
            string adminAddress = "0xba83c830d684df6eaaa8f143b1e853ea1e2b67522154780f9378d30b7f9d9d59";
            var rpcClient = new UnityWebRequestRpcClient(SuiConstants.TESTNET_FULLNODE);
            IJsonRpcApiClient Client = new SuiJsonRpcApiClient(rpcClient);
            ISigner signerE = new Signer(Client, adminKey);

           INftProtocolClient NftProtocolClient = new NftProtocolClient(Client, SuiWallet.GetActiveKeyPair());
            
            
            var moduleAd = "farming";
            var functionAd = "add_exchanger";
            var typeArgsAd = System.Array.Empty<string>();
            var exchanger = new List<byte>(SuiWallet.GetActiveKeyPair().PublicKey);

            var argsAd = new object[] {
                _farmerDataAddress,
                exchanger,
            };

            var gasBudgetD = BigInteger.Parse("10000000");
           
            var rpcResultD = await SuiApi.Client.MoveCallAsync(adminAddress, _packageAddress, moduleAd, functionAd, typeArgsAd, argsAd, gasBudgetD);
             
              var keyPair = adminKey;

                var txBytes = rpcResultD.Result.TxBytes;
                var rawSigner = new RawSigner(keyPair);
                
                var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));
            
               var rpcResult2 = await Client.ExecuteTransactionBlockAsync(txBytes, new[] { signature.Value }, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);

 
            Debug.Log("Exchanger: " + rpcResult2.RawRpcResponse);

            return rpcResult2;

        }
        public async static Task<bool> CheckInFarming() {
            bool result = false;
            var rpcResult =  await SuiApi.Client.GetObjectAsync("0xb2b02c51794af3250094b527047b86d8ad0df9b889cff4af8e336d0941dea148", new ObjectDataOptions { ShowContent = true});
            string farmerData = JsonConvert.SerializeObject(rpcResult.Result.Data.Content, Formatting.Indented);
           
            JObject farmerDataObject = JObject.Parse(farmerData);
            JArray famerDataArray =(JArray) farmerDataObject.SelectToken("fields.farmers.fields.contents");
            
            foreach(JObject farmer in famerDataArray) {
                
                if(farmer.SelectToken("fields.key").ToString().Contains(SuiWallet.GetActiveAddress())) {
                    if(bool.Parse(farmer.SelectToken("fields.value.fields.farming").ToString())) 
                       result = true;
                }
            }
            return result;
        }

        public async static Task<RpcResult<TransactionBlockResponse>> CancelFarming() {
            
            var signer = SuiWallet.GetActiveAddress();

            var module = "farming";
            var function = "cancel_farming";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _farmerDataAddress,
                _clockAddress
            };
           
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

            var executeResult =  await Execute(rpcResult);

            return executeResult;

        }

        public async static Task<RpcResult<TransactionBlockBytes>> StartFarming()
        {
            //Check In Farming End remove it;
            bool isFarming = await CheckInFarming();
            if(isFarming == true) {
                var cancelFarmingResult = await CancelFarming();
                if(cancelFarmingResult.IsSuccess == false) {
                    UIManager.Instance.ShowAlert("You don't have enough SUI balance for gas fees !",AlertType.Warning);
                }
            }

            var exchangerTest = await Add_Exchanger();

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

        public static async Task<RpcResult<TransactionBlockBytes>> CraftGlass(List<string> stoneList) {
             var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "crafting_item";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _craftingDataAddress,
                _minterDataAddress,
                stoneList,
                _glassSymbol
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
        




    

        public async static Task<RpcResult<TransactionBlockBytes>> EquipWeapon(string address, string type) {
           
            var weaponObjectResult = await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
            
            MoveObjectData weaponObjectData = weaponObjectResult.Result.Data.Content as MoveObjectData;

            var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "equip";
            var typeArgs = new List<string>{weaponObjectData.Type};
            var args = new object[] {
                InventoryManager.Instance.CurrentHunterAddressInUse,
                address,
                type
            };
           
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);
            Debug.Log(rpcResult.RawRpcResponse);

            return rpcResult;
        }


        public async static Task<RpcResult<TransactionBlockBytes>> UnEquipWeapon(string address,string type) {
           
            var weaponObjectResult = await SuiApi.Client.GetObjectAsync(address,ObjectDataOptions.ShowAll());
            Debug.Log(weaponObjectResult.RawRpcResponse);
            MoveObjectData weaponObjectData = weaponObjectResult.Result.Data.Content as MoveObjectData;

            var signer = SuiWallet.GetActiveAddress();
            var module = "crafting";
            var function = "cancel_equip";
            var typeArgs = new List<string>{weaponObjectData.Type};
            var args = new object[] {
                InventoryManager.Instance.CurrentHunterAddressInUse,
                type
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
