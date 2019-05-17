using System;
using System.Net.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TimerSample
{
    public static class HealthCheck
    {
        static HttpClient cl = new HttpClient();
        // ヘルスチェックを行うURL
#if DEBUG
        static string ServerName = "localhost:7071";
        static string targetUrl = $"http://{ServerName}/api/HealthCheckTarget?name=healthcheck";
#else
        static string ServerName = "sample-azfunc-adv-timersample.azurewebsites.net";
        static string targetUrl = "";
#endif

        // ヘルスチェックの結果を書き込む Cosmos DBの設定
        // 本来はFunction Appの設定から読み込む
#if DEBUG
        static string Endpoint = "https://localhost:8081";
        static string Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
#else
        static string Endpoint = "https://sample-azfunc-cosmosdb.documents.azure.com:443/";
        static string Key = "";
#endif
        static string DatabaseId = "HealthCheck";
        static string CollectionId = "Results";
        /// <summary>
        /// ヘルスチェックを行う関数
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        [FunctionName("HealthCheck")]
        public static async void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"HealthCheck called: {DateTime.Now}");
            // ヘルスチェック対象のWeb APIを呼び出す
            var result = "";
            try
            {
                /// 対象をヘルスチェックする
                /// 指定のURLを呼び出して、応答を見る簡単な方式
                var res = await cl.GetAsync(targetUrl);
                result = await res.Content.ReadAsStringAsync();
                log.LogInformation("HealthCheck response: " + result);
            }
            catch
            {
                /// ヘルスチェック先が応答しない場合、例外が発生する
                log.LogInformation("HealthCheck response: ERROR");
                result = "ERROR";
            }
            // 結果をCosmos DBに書き込む
            var data = new HealthCehckEntry()
            {
                Region = "Japan",
                ServerName = ServerName,
                Result = result,
                CreatedAt = DateTime.Now
            };
            var client = new DocumentClient(new Uri(Endpoint), Key);
            await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), data);
            /// Cosmos DB の書き込みトリガーを利用して、エラー発生時のアクションを追加しても良い
        }
    }
    public class HealthCehckEntry
    {
        public string Region { get; set; }
        public string ServerName { get; set; }
        public string Result { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
