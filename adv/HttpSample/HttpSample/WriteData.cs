using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HttpSample.Models;
using Microsoft.EntityFrameworkCore;

namespace HttpSample
{
    public static class WriteData
    {
        [FunctionName("WriteData")]                                                 // �@
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]		// �A
	        HttpRequest req, ILogger log)
        {
            log.LogInformation("called WriteData");                                 // �B
            // POST�f�[�^����p�����[�^�[���擾
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync(); // �C
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string pno = data?.pno;
            string status = data?.status;
            // �p�����[�^�̃`�F�b�N
            if (pno == null || status == null)                                      // �D
            {
                return new BadRequestObjectResult(
                    "ERROR: �Ј��ԍ�(pno)�Ə��(status)���w�肵�ĉ�����");
            }
            // �f�[�^���X�V
            var context = new azuredbContext();                                     // �E
            var item = await context.Persons.FirstOrDefaultAsync(t => t.PersonNo == pno);
            if (item == null)                                                       // �F
            {
                return new BadRequestObjectResult(
                    "ERROR: �Ј��ԍ�(pno)��������܂���");
            }
            // �o�Ώ�Ԃ��X�V
            item.Status = status;                                                   // �G
            item.ModifiedAt = DateTime.Now;
            context.Persons.Update(item);
            await context.SaveChangesAsync();
            // �X�V���ʂ�Ԃ�
            return new OkObjectResult(                                              // �H
                "SUCCESS: �X�V���܂��� " + DateTime.Now.ToString());
        }
    }
}
