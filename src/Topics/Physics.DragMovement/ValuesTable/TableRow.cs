﻿using Physics.Shared.UI.Services.ValuesTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.DragMovement.ValuesTable
{
    public class TableRow : ValuesTableRowBase
    {
        private const string ZeroFormatting = "0.###";

        public string Time { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string V { get; set; }
        public string F { get; set; }
        //public string A { get; set; }
        //public string C { get; set; }

        public TableRow(float time, float x, float y, float v, float f)
        {
            Time = time.ToString(ZeroFormatting);
            X = x.ToString(ZeroFormatting);
            Y = y.ToString(ZeroFormatting);
            V = v.ToString(ZeroFormatting);
            F = f.ToString(ZeroFormatting);
            //C = a.ToString(ZeroFormatting);
        }

        protected override IEnumerable<string> GetCellValuesInOrder()
        {
            yield return Time;
            yield return X;
            yield return Y;
            yield return F;
            yield return V;
            //yield return C;
        }
    }
}
