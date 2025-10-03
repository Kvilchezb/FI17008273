
using Microsoft.AspNetCore.Mvc;
using BinaryOperationsApp.Models;
using System;

namespace BinaryOperationsApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new BinaryModel());
        }

        [HttpPost]
        public IActionResult Index(BinaryModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.A) || string.IsNullOrEmpty(model.B) ||
                model.A.Length % 2 != 0 || model.B.Length % 2 != 0)
            {
                ModelState.AddModelError("", "La longitud debe ser múltiplo de 2 y no puede estar vacía.");
                return View(model);
            }

            string a = model.A.PadLeft(8, '0');
            string b = model.B.PadLeft(8, '0');

            int aDec = Convert.ToInt32(a, 2);
            int bDec = Convert.ToInt32(b, 2);

            string and = BinaryOp(a, b, (x, y) => x & y);
            string or = BinaryOp(a, b, (x, y) => x | y);
            string xor = BinaryOp(a, b, (x, y) => x ^ y);

            int sum = aDec + bDec;
            int mul = aDec * bDec;

            model.ResultTableHtml = GenerateTable(a, b, and, or, xor, sum, mul);
            return View(model);
        }

        private string BinaryOp(string a, string b, Func<int, int, int> op)
        {
            char[] result = new char[8];
            for (int i = 0; i < 8; i++)
            {
                int bitA = a[i] - '0';
                int bitB = b[i] - '0';
                result[i] = (op(bitA, bitB)).ToString()[0];
            }
            return new string(result);
        }

        private string GenerateTable(string a, string b, string and, string or, string xor, int sum, int mul)
{
    string FormatBin(string bin) => bin.TrimStart('0') == "" ? "0" : bin.TrimStart('0');
    string FormatOct(string bin) => Convert.ToString(Convert.ToInt32(bin, 2), 8);
    string FormatDec(string bin) => Convert.ToInt32(bin, 2).ToString();
    string FormatHex(string bin) => Convert.ToString(Convert.ToInt32(bin, 2), 16).ToUpper();

    string sumBin = Convert.ToString(sum, 2);
    string mulBin = Convert.ToString(mul, 2);

    return $@"
<style>
    table {{
        border-collapse: collapse;
        width: 100%;
        margin-top: 20px;
    }}
    th, td {{
        border: 1px solid #ccc;
        padding: 8px;
        text-align: center;
    }}
    th {{
        background-color: #f2f2f2;
    }}
    tr:nth-child(even) {{
        background-color: #f9f9f9;
    }}
</style>

<table>
<tr><th></th><th>Bin</th><th>Oct</th><th>Dec</th><th>Hex</th></tr>
<tr><td>a</td><td>{a.PadLeft(8, '0')}</td><td>{FormatOct(a)}</td><td>{FormatDec(a)}</td><td>{FormatHex(a)}</td></tr>
<tr><td>b</td><td>{b.PadLeft(8, '0')}</td><td>{FormatOct(b)}</td><td>{FormatDec(b)}</td><td>{FormatHex(b)}</td></tr>
<tr><td>a AND b</td><td>{FormatBin(and)}</td><td>{FormatOct(and)}</td><td>{FormatDec(and)}</td><td>{FormatHex(and)}</td></tr>
<tr><td>a OR b</td><td>{FormatBin(or)}</td><td>{FormatOct(or)}</td><td>{FormatDec(or)}</td><td>{FormatHex(or)}</td></tr>
<tr><td>a XOR b</td><td>{FormatBin(xor)}</td><td>{FormatOct(xor)}</td><td>{FormatDec(xor)}</td><td>{FormatHex(xor)}</td></tr>
<tr><td>a + b</td><td>{sumBin}</td><td>{Convert.ToString(sum, 8)}</td><td>{sum}</td><td>{Convert.ToString(sum, 16).ToUpper()}</td></tr>
<tr><td>a • b</td><td>{mulBin}</td><td>{Convert.ToString(mul, 8)}</td><td>{mul}</td><td>{Convert.ToString(mul, 16).ToUpper()}</td></tr>
</table>";
}

    }
}
