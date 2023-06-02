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

        public static async Task<RpcResult<TransactionBlockResponse>> MintHunterNFT() {

            var keypair = SuiWallet.GetActiveKeyPair();
            var nftProtocolClient = new NftProtocolClient(SuiApi.Client, SuiWallet.GetActiveKeyPair());
           
            var txParams = new MintSuitradersNft()
            {
                Attributes = new Dictionary<string, object>()
                {
                    { "nft_ob", _nftCharacterId },
                },
                ModuleName =  "SpaceHunter",
                Function = "mint_nft",
                PackageObjectId = _nftCharacterPackageId,
                Signer = keypair.PublicKeyAsSuiAddress,
            };

            var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
   
            return mintRpcResult;
        }   
        public static async Task<RpcResult<TransactionBlockResponse>> MintMineral() {
            
            ulong amount = 1;

            var keypair = SuiWallet.GetActiveKeyPair();
            var nftProtocolClient = new NftProtocolClient(SuiApi.Client, SuiWallet.GetActiveKeyPair());

            var txParams = new MintSuitradersNft()
            {
                Attributes = new Dictionary<string, object>()
                {
                    { "arg0", _nftMineralId},
                    { "arg1", amount}
                },
                ModuleName =  "stone",
                Function = "claim_stone",
                PackageObjectId = _nftMineralPackageId,
                Signer = keypair.PublicKeyAsSuiAddress,
            };

            var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
   
            return mintRpcResult;
        }   
        
        public static async Task<RpcResult<Page_for_SuiObjectResponse_and_ObjectID>> GetAllNFT() {
            ObjectDataOptions optionsReq = new ObjectDataOptions();
            optionsReq.ShowContent = true;
            var allObject = await SuiApi.Client.GetOwnedObjectsAsync(SuiWallet.GetActiveAddress(),  
                new ObjectResponseQuery() { Options =  optionsReq},null,null);
            return allObject;
        }
      
    }

}
