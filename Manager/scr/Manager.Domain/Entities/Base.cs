using System.Collections.Generic;
using System.Text;

namespace Manager.Domain.Entities{
    //classe abstract não pode ser instanciada apenas herdada
    public abstract class Base{
        public long Id { get; set; }

        internal List<string> _errors;
        //IReadOnlyCollection ou seja apenas ira ler os erros não pode alterar
        public IReadOnlyCollection<string> Errors => _errors;

        public abstract bool Validate();
    }
}
