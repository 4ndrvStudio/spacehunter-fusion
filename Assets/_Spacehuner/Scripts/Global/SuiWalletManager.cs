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

        private static string _packageAddress = "0x7167be0deb466c9c0650e7fbad9b283ce1ac1b0f97a0122696b88bf6bb0a128b";
        private static string _farmerDataAddress = "0xabdb5b5d572e217841d2fe1bdcd1de70ec53ac2191cc40093e8eb64f7cae3425";
        private static string _minterDataAddress = "0x732f097b6791cc10d12fe4822372b203b952d9deb610ef10e93a9cf7aa37e194";
        private static string _craftingDataAddress  = "0xc49b47fba60982103801714603816120c9417eeb5d745e70545104810b936eee";

        private static string _hunterAddress {get; set;}
        private static string _hunterSymbol = "dst";
        private static string _swordSymbol = "dst";

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
            
            byte[] symbolByte = encoder.Serialize("vector<string>", symbolStone);
          
            byte[] combinedBytes = expByte.Concat(amountByte).Concat(symbolByte).ToArray();

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
                symbolStone
            };
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

            Debug.Log(rpcResult.RawRpcRequest);

            return rpcResult;
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
