using System.Collections.Generic;
using DocumentsKM.Model;

namespace DocumentsKM.Data
{
    public interface IPositionRepo
    {
        Position GetPositionByCode(uint code);
    }
}
