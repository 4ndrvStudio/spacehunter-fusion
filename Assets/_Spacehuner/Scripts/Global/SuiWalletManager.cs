using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Suinet.NftProtocol;
using Suinet.NftProtocol.Examples;
using System.Threading.Tasks;
using Suinet.Rpc;
using Suinet.Rpc.Types;
using Suinet.Faucet;

namespace SH
{
    public class SuiWalletManager : MonoBehaviour
    {
        private static string _nftCharacterPackageId ="0x201e77838a6f75d1e6b6808052d0049bb38e880fba41fd7b6d2cde99150edd6a";
        private static string _nftSharedObjectId = "0x05843aeab83f3f144d138d3294eec3037dad24e58e05342714bed90b4db59bed";
       
        public static async Task<RpcResult<TransactionBlockResponse>> MintHunterNFT() {

            var keypair = SuiWallet.GetActiveKeyPair();
            var nftProtocolClient = new NftProtocolClient(SuiApi.Client, SuiWallet.GetActiveKeyPair());

            var txParams = new MintSuitradersNft()
            {
                Attributes = new Dictionary<string, object>()
                {
                    { "nft_ob", _nftSharedObjectId },
                },
                ModuleName =  "SpaceHunter",
                Function = "mint_nft",
                PackageObjectId = _nftCharacterPackageId,
                Signer = keypair.PublicKeyAsSuiAddress,
            };

            var mintRpcResult = await nftProtocolClient.MintNftAsync(txParams, null);
   
            return mintRpcResult;
        }   

        public static async Task<string> GetSuiWalletBalance() 
        {    
            string coinType = "0x2::sui::SUI";

            var balanceResult = await SuiApi.Client.GetBalanceAsync(SuiWallet.GetActiveAddress(),coinType);
         
            float balance =  ((float)balanceResult.Result.TotalBalance)/1000000000;
          
            
            return balance.ToString("0.####");
        }
    }

}
