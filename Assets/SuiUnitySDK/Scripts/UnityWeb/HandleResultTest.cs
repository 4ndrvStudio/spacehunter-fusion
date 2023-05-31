
using System.Collections;
using System.Collections.Generic;
namespace HandleResultTest
{
    public class Result
    {
        public string digest { get; set; }
        public Transaction transaction { get; set; }
    }

    public class Transaction
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public TransactionDetails transaction { get; set; }
    }

    public class TransactionDetails
    {
        public string kind { get; set; }
        public List<Input> inputs { get; set; }
        public List<Transactions> transactions { get; set; }
    }

    public class Input
    {
        public string type { get; set; }
        public string objectType { get; set; }
        public string objectId { get; set; }
        public string initialSharedVersion { get; set; }
        public bool mutable { get; set; }
    }

    public class Transactions
    {
        public MoveCall MoveCall { get; set; }
    }

    public class MoveCall
    {
        public string package { get; set; }
        public string module { get; set; }
        public string function { get; set; }
        public List<Argument> arguments { get; set; }
    }

    public class Argument
    {
        public string Input { get; set; }
    }


    public class Effects {
        public Created created;
    }

    public class Created {
        public Reference reference;
    }

    public class Reference {
        public string objectId;
    }
}
