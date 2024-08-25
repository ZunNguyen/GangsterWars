using Cysharp.Threading.Tasks;

namespace Sources.Services
{
    public interface IService
    {
        public enum Result
        {
            Success,
            Failure
        }

        UniTask<Result> Execute();
    }

    public abstract class Service : IService
    {
        public abstract UniTask<IService.Result> Execute();

        public async UniTask Run()
        {
            var result = IService.Result.Failure;
            try
            {
                result = await Execute();
            }
            catch
            {

            }
            Finish(result);
        }

        private void Finish(IService.Result result)
        {

        }
    }
}

