using Suinet.Rpc.Types;
using Suinet.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using Suinet.NftProtocol.TransactionBuilders;

namespace Suinet.NftProtocol.Examples
{
    public class MintSuitradersNft : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string Function { get; set; }

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public string Recipient { get; set; }

        public MoveCallTransaction BuildMoveCallTransaction(string gas = null, ulong gasBudget = 10000, ExecuteTransactionRequestType RequestType = ExecuteTransactionRequestType.WaitForLocalExecution)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = ModuleName,
                Function = Function,
                TypeArguments = Array.Empty<string>(),
                Arguments = ArgumentBuilder.BuildArguments(
                    Attributes.Values.ToArray()
                    ),
                Gas = gas,
                GasBudget = gasBudget,
                RequestType = RequestType
            };
        }


    }
}
