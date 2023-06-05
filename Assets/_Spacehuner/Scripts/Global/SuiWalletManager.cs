using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Suinet.NftProtocol;
using Suinet.NftProtocol.Examples;
using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Suinet.Faucet;
using SH.Multiplayer;
using Newtonsoft.Json;
using BigInteger = System.Numerics.BigInteger;
using Suinet.Wallet;


namespace SH
{
    public class SuiWalletManager : MonoBehaviour
    {
        //character
        private static string _nftCharacterPackageId ="0x201e77838a6f75d1e6b6808052d0049bb38e880fba41fd7b6d2cde99150edd6a";
        private static string _nftCharacterId = "0x05843aeab83f3f144d138d3294eec3037dad24e58e05342714bed90b4db59bed";
       
        //mineral
        private static string _nftMineralPackageId ="0x50d80703f4f3ad80284d999a49f6978ed52dfa37965ec9970daf8e541de1b3bc";
        private static string _nftMineralId = "0x38234dbc1abc10f24394d289e9f9b69b6a2839db574227cd05c97a05302c7726";

       
        public static async Task<string> GetSuiWalletBalance() 
        {    
            string coinType ="0x2::sui::SUI";


            var balanceResult = await SuiApi.Client.GetBalanceAsync(SuiWallet.GetActiveAddress(),coinType);
            
            float balance =  ((float)balanceResult.Result.TotalBalance)/1000000000;
          
            
            return balance.ToString("0.#########");
        }

        public static async Task<RpcResult<TransactionBlockResponse>> MintHunterNFT() 
        {
            var mintRpcResult = new RpcResult<TransactionBlockResponse>();
            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = _nftCharacterPackageId;
            var module = "SpaceHunter";
            var function = "mint_nft";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { _nftCharacterId};
            var gasBudget = BigInteger.Parse("1000000");
            
            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasBudget);
            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();

                var txBytes = rpcResult.Result.TxBytes;
                var rawSigner = new RawSigner(keyPair);
                var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));

                mintRpcResult = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] {signature.Value}, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);
            }
            else
            {
                Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
            }

            return mintRpcResult;
        }   
        public static async Task<RpcResult<TransactionBlockResponse>> MintMineral() {
            
            ulong amount = 1;

            var mintRpcResult = new RpcResult<TransactionBlockResponse>();

            var signer = SuiWallet.GetActiveAddress();
            var packageObjectId = _nftMineralPackageId;
            var module = "stone";
            var function = "claim_stone";
            var typeArgs = System.Array.Empty<string>();
            var args = new object[] { _nftMineralId, amount};
            var gasBudget = BigInteger.Parse("1000000");
            
            var rpcResult = await SuiApi.Client.MoveCallAsync(signer, packageObjectId, module, function, typeArgs, args, gasBudget);
            if (rpcResult.IsSuccess)
            {
                var keyPair = SuiWallet.GetActiveKeyPair();

                var txBytes = rpcResult.Result.TxBytes;
                var rawSigner = new RawSigner(keyPair);
                var signature = rawSigner.SignData(Intent.GetMessageWithIntent(txBytes));

                mintRpcResult = await SuiApi.Client.ExecuteTransactionBlockAsync(txBytes, new[] {signature.Value}, TransactionBlockResponseOptions.ShowAll(), ExecuteTransactionRequestType.WaitForLocalExecution);
            }
            else
            {
                Debug.LogError("Something went wrong with the move call: " + rpcResult.ErrorMessage);
            }
   
            return mintRpcResult;
        }   
        
        public static async Task<RpcResult<Page_for_SuiObjectResponse_and_ObjectID>> GetAllNFT() {
           
            ObjectDataOptions optionsReq = new ObjectDataOptions();
           
            optionsReq.ShowContent = true;
            var allObject = await SuiApi.Client.GetOwnedObjectsAsync(SuiWallet.GetActiveAddress(),  
                new ObjectResponseQuery() { Options =  optionsReq},null,null);
            Debug.Log(allObject.RawRpcResponse);
            return allObject;
        }
      
    }

}
