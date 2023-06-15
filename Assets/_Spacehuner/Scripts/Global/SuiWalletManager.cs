using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

using System.Numerics;



namespace SH
{
    public class SuiWalletManager : MonoBehaviour
    {
        private static ParticleSystem _ser;

        private static string _clockAddress = "0x0000000000000000000000000000000000000000000000000000000000000006";

        private static string _packageAddress = "0x7167be0deb466c9c0650e7fbad9b283ce1ac1b0f97a0122696b88bf6bb0a128b";
        private static string _farmerDataAddress = "0xabdb5b5d572e217841d2fe1bdcd1de70ec53ac2191cc40093e8eb64f7cae3425";
        private static string _minterDataAddress = "0x732f097b6791cc10d12fe4822372b203b952d9deb610ef10e93a9cf7aa37e194";

        private static string _hunterAddress = "0x54efae60edaf625ea6303a7cfdf42633dc785eeb14867c3869fc513d59f6bd8c";


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

        public async static Task<RpcResult<TransactionBlockResponse>> StartFarming()
        {
            var mintRpcResult = new RpcResult<TransactionBlockResponse>();

            var signer = SuiWallet.GetActiveAddress();
            var module = "farming";
            var function = "start_farming";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] {
                _farmerDataAddress,
                _hunterAddress,
                _clockAddress
            };
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

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

            Debug.Log(mintRpcResult.RawRpcResponse);


            return mintRpcResult;
        }

        public async static Task<RpcResult<TransactionBlockResponse>> EndFarming()
        {
            var mintRpcResult = new RpcResult<TransactionBlockResponse>();

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

            ulong expDefine = 1000;

            byte[] expByte = encoder.Serialize("u64", expDefine);

            byte[] amountByte = encoder.Serialize("vector<u8>", new List<byte> { 1 });
            
            byte[] symbolByte = encoder.Serialize("vector<string>", new List<string> { "red" });

          
            byte[] combinedBytes = expByte.Concat(amountByte).Concat(symbolByte).ToArray();

            var bcsSignature = new List<byte>(SuiWallet.GetActiveKeyPair().Sign(combinedBytes));
           
            ulong amount = 1000;

            var exchanger = new List<byte>(SuiWallet.GetActiveKeyPair().PublicKey);

            var args = new object[] {
                _farmerDataAddress,
                _minterDataAddress,
                _hunterAddress,
                _clockAddress,
                bcsSignature,
                exchanger,
                amount,
                new List<ulong>{1},
                new List<string>{"red"}
            };
            var gasBudget = BigInteger.Parse("10000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, _packageAddress, module, function, typeArgs, args, gasBudget);

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

            Debug.Log(mintRpcResult.RawRpcResponse);


            return mintRpcResult;
        }

        public static async Task<RpcResult<TransactionBlockResponse>> MintHunterNFT()
        {
            var mintRpcResult = new RpcResult<TransactionBlockResponse>();
            var signer = SuiWallet.GetActiveAddress();

            var packageObjectId = _nftCharacterPackageId;
            var module = "SpaceHunter";
            var function = "mint_nft";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { _nftCharacterId };
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




        public async static Task<RpcResult<TransactionBlockResponse>> EndFarmMint()
        {

            var mintRpcResult = new RpcResult<TransactionBlockResponse>();

            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = "0x429f802f33fbf910c07ef2eeca46e1c788af95e6b8f6eb82ec203170305fb4ff";
            var module = "farming";
            var function = "test_u";
            var typeArgs = System.Array.Empty<string>();

            var formatter = new BinaryFormatter();

            // Serialize the values into memory streams
            var expStream = new MemoryStream();
            formatter.Serialize(expStream, 1000);
            var amountStream = new MemoryStream();
            formatter.Serialize(amountStream, new List<ulong> { 1 });
            var symbolStream = new MemoryStream();
            formatter.Serialize(symbolStream, new List<string> { "red" });

            // Combine the memory streams
            var combinedStream = new MemoryStream();
            var amount_bytesTe = new MemoryStream();

            expStream.Position = 0;
            expStream.CopyTo(combinedStream);
            amountStream.Position = 0;
            amountStream.CopyTo(combinedStream);
            amountStream.CopyTo(amount_bytesTe);
            symbolStream.Position = 0;
            symbolStream.CopyTo(combinedStream);


            // BcsEncoder encoder = new BcsEncoder();


            // encoder.RegisterType<List<ulong>>("vector<u64>", (writer, data, options, parameters) =>
            // {
            //     List<ulong> vector = (List<ulong>)data;
            //     writer.WriteVec(vector, (w, item, index, count) =>
            //     {
            //         w.WriteUInt64(item);
            //     });
            //     return null;
            // });


            // byte[] expe = encoder.Serialize("u64", 1000);

            // List<ulong> vector = new List<ulong> { 1 };
            // byte[] serializedData = encoder.Serialize("vector<u64>", vector);



            // Convert the combined memory stream to a byte array
            byte[] combinedBytes = combinedStream.ToArray();

            ulong amount = 1000;
            // ulong[] amount_bytesTe = new ulong[1] {1};
            // string[] symbol_bytes = new string[1] {"red"};

            List<ulong> vector = new List<ulong>();
            vector.Add(1);


            byte[] byteArray = new byte[vector.Count * sizeof(ulong)];

            Buffer.BlockCopy(vector.ToArray(), 0, byteArray, 0, byteArray.Length);

            string base64String = Convert.ToBase64String(byteArray);
            Console.WriteLine(base64String);

            var signTest = SuiWallet.GetActiveKeyPair().Sign(combinedBytes);
            // var args = new object[] {
            //     "0xbc153d4e2397d2718e950db66e9c1786dae6b54862ab2d2a6b877e921ab957ea",
            //     "0x85cd1d2e420b086c244f8ae7e88225e04126a0a7f81a29d1c49e26ca239f61e8",
            //     "0x86c94391fc4a946ad8b9d681c166d6aa6362a1e4fe6c76a9cb58374ba361e198",
            //     "0x0000000000000000000000000000000000000000000000000000000000000006",
            //     signTest,
            //     SuiWallet.GetActiveKeyPair().PublicKey,
            //     amount,
            //     byteArray,
            //      new List<string>{"red"}
            // };
            var args = new object[] {
                new List<ulong>() {1}
            };
            var gasBudget = BigInteger.Parse("1000000");

            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasBudget);

            Debug.Log(rpcResult.RawRpcRequest);

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

            Debug.Log(mintRpcResult.RawRpcResponse);

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
