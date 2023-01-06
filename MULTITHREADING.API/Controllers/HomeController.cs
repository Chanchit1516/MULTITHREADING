using Microsoft.AspNetCore.Mvc;
using MULTITHREADING.API.DTOs.Multithread;
using MULTITHREADING.API.DTOs.Multithread.Responses;

namespace MULTITHREADING.API.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IServiceProvider _provider;

        public HomeController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [Route("LoopMultithread")]
        [HttpGet]
        public async Task<bool> LoopMultithread()
        {
            bool isSuccess = false;
            List<MockupdataDTO> list = new List<MockupdataDTO>();
            MockupdataDTO model = new MockupdataDTO();
            var tasks = new List<Task<TaskResult>>();

            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(CoreProcess(model = new MockupdataDTO() { Id = i, Name = "Name-" + i, Description = "Description" + i, Price = i + 1, CreatedBy = "Oat" + 1 }));
            }
            Task t = Task.WhenAll(tasks);
            try
            {
                t.Wait();
            }
            catch { }

            if (t.Status == TaskStatus.RanToCompletion)
            {
                isSuccess = true;
            }
            else if (t.Status == TaskStatus.Faulted)
            {
                isSuccess = false;
                var taskResult = tasks.FirstOrDefault(i => i.Result.TaskStatus == false && !string.IsNullOrEmpty(i.Result.TaskMessage));
                throw new Exception($"process task fail code:003 | {taskResult.Result.TaskMessage} | {taskResult.Result.TaskStackTrace}");
            }
            if (tasks.Any(i => i.Result.TaskStatus == false))
            {
                isSuccess = false;
                var taskResult = tasks.FirstOrDefault(i => i.Result.TaskStatus == false && !string.IsNullOrEmpty(i.Result.TaskMessage));
                throw new Exception($"process task fail code:003 | {taskResult.Result.TaskMessage} | {taskResult.Result.TaskStackTrace}");
            }

            return isSuccess;
        }

        private async Task<TaskResult> CoreProcess(MockupdataDTO mockupData)
        {
            return await Task.Run(() =>
            {
                try
                {
                    #region Resolve Instances dependencies With ASP.NET Core DI
                    //การทำ multithread และมีการใช้ unitofwork ในการ insert หรือ update data จำเป็นต้อง new Instances unitofwork ใหม่ตลอด ไม่สามารถใช้ Instances เดียวกันได้เพราะมันจะทำ loop พร้อมกัน

                    //var _unitOfWork = (IUnitOfWork)_provider.GetService(typeof(IUnitOfWork));
                    #endregion

                    #region Example await
                    //var isComplaint = _unitOfWork.ComplaintRepository.IsComplaintByCaseNoAsync(caseNo).ConfigureAwait(false).GetAwaiter().GetResult();
                    #endregion


                    #region Commit transation
                    //_unitOfWork.Commit();
                    #endregion

                    return new TaskResult
                    {
                        TaskStatus = true
                    };
                }
                catch (Exception ex)
                {
                    return new TaskResult
                    {
                        TaskStatus = false,
                        TaskMessage = ex.Message,
                        TaskStackTrace = ex.StackTrace,
                    };
                }
            });
        }


    }
}
