using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;
using TestNS.Models;

namespace TestNS.Controllers;

[Route("/api/[controller]")]
[ApiController]
public partial class TestController(ILogger<TestController> log)
    : ControllerBase
{
    /// <summary>
    /// test index
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(string), 200)]
    public string Index()
    {
        var c = new global::zkemkeeper.CZKEMClass();
        return "1";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost(nameof(AA))]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<object> AA(AAModel aa)
    {
        var builder = new StringBuilder();

        var axCZKEM1 = new zkemkeeper.CZKEMClass();
        var bIsConnected = false;
        var idwErrorCode = 0;
        var iMachineNumber = 1;
        var ls = new List<object>();
        try
        {
            bIsConnected = axCZKEM1.Connect_Net(aa.ip, aa.port); // 可以多次调用
            if (!bIsConnected)
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
            }

            // 连接成功后读取设备考勤数据
            if (idwErrorCode == 0)
            {
                string sdwEnrollNumber = "";
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;

                axCZKEM1.EnableDevice(iMachineNumber, false);
                if (axCZKEM1.ReadTimeGLogData(iMachineNumber, aa.vdate1, aa.vdate2))
                {
                    while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
                       out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode)) //get records from the memory
                    {
                        ls.Add(new
                        {
                            EnrollNumber = sdwEnrollNumber,
                            VerifyType = idwVerifyMode.ToString(),
                            InOutMode = idwInOutMode.ToString(),
                            Date = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond).ToString("yyyy-MM-dd HH:mm:ss"),
                            WorkCode = idwWorkcode.ToString(),
                        });
                    }
                }
                else
                {
                    axCZKEM1.GetLastError(ref idwErrorCode);
                    if (idwErrorCode != 0)
                    {
                        builder.AppendLine($"读取数据失败,errorcode={idwErrorCode}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (idwErrorCode == 0)
            {
                idwErrorCode = -1;
                builder.Append(ex.Message);
            }
        }
        finally
        {
            if (bIsConnected)
            {
                try { axCZKEM1.Disconnect(); }
                catch { }
            }
        }

        return new
        {
            idwErrorCode,
            kqLs = ls,
        };
    }
}
