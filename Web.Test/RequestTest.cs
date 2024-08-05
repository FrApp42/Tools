using FrApp42.Web.API;
using System.Text;
using Web.Test.Models;

namespace Web.Test
{
	[TestClass]
	public class RequestTest
	{
		private const string TestGetUrl = "https://httpbin.org/get";
		private const string TestDeleteUrl = "https://httpbin.org/delete";
		private const string TestPatchUrl = "https://httpbin.org/patch";
		private const string TestPostUrl = "https://httpbin.org/post";
		private const string TestPutUrl = "https://httpbin.org/put";

		private const string TestGetJpegImageUrl = "https://httpbin.org/image/jpeg";
		private const string TestGetPngImageUrl = "https://httpbin.org/image/png";
		private const string TestGetSvgImageUrl = "https://httpbin.org/image/svg";
		private const string TestGetWebpImageUrl = "https://httpbin.org/image/webp";

		[TestMethod]
		public async Task SendGetRequest()
		{
			Request request = new(TestGetUrl, HttpMethod.Get);
			Result<HttpBinGetResponse> result = await request.Run<HttpBinGetResponse>();

			AssertResponse(result, TestGetUrl);
		}

		[TestMethod]
		public async Task SendDeleteRequest()
		{
			Request request = new(TestDeleteUrl, HttpMethod.Delete);
			Result<HttpBinDeleteResponse> result = await request.Run<HttpBinDeleteResponse>();

			AssertResponse(result, TestDeleteUrl);
		}

		[TestMethod]
		public async Task SendPatchRequest()
		{
			Request request = new(TestPatchUrl, HttpMethod.Patch);
			Result<HttpBinPatchResponse> result = await request.Run<HttpBinPatchResponse>();

			AssertResponse(result, TestPatchUrl);
		}

		[TestMethod]
		public async Task SendPostRequest()
		{
			Request request = new(TestPostUrl, HttpMethod.Post);
			Result<HttpBinPostResponse> result = await request.Run<HttpBinPostResponse>();

			AssertResponse(result, TestPostUrl);
		}

		[TestMethod]
		public async Task SendPutRequest()
		{
			Request request = new(TestPutUrl, HttpMethod.Put);
			Result<HttpBinPutResponse> result = await request.Run<HttpBinPutResponse>();

			AssertResponse(result, TestPutUrl);
		}

		[TestMethod]
		public async Task SendPostRequestWithFile()
		{
			byte[] fileBytes = Encoding.UTF8.GetBytes("Hello world");
			string fileName = "test.txt";

			Request request = new(TestPostUrl, HttpMethod.Post);
			request
				.SetContentType("text/plain")
				.AddDocumentBody(fileBytes, fileName);

			Result<HttpBinPostFileResponse> result = await request.RunDocument<HttpBinPostFileResponse>();

			AssertResponse(result, TestPostUrl);
			StringAssert.Contains(result.Value.Data, "Hello world", "File content should contain 'Hello World'");
		}

		[TestMethod]
		public async Task SendJpegImageRequest()
		{
			await SendImageRequest("image/jpeg", "jpeg");
		}

		[TestMethod]
		public async Task SendPngImageRequest()
		{
			await SendImageRequest("image/png", "png");
		}

		[TestMethod]
		public async Task SendSvgImageRequest()
		{
			await SendImageRequest("image/svg+xml", "svg");
		}

		[TestMethod]
		public async Task SendWebpImageRequest()
		{
			await SendImageRequest("image/webp", "webp");
		}

		private async Task SendImageRequest(string acceptType, string imageType)
		{
			Request request = new(TestGetWebpImageUrl, HttpMethod.Get);
			request
				.AddHeader("Accept", acceptType);

			Result<byte[]> result = await request.RunGetBytes();

			AssertImageResponse(result, imageType);
		}

		private void AssertResponse<T>(Result<T> result, string expectedUrl) where T : class
		{
			Assert.AreEqual(200, result.StatusCode, "Status code should be 200");
			Assert.IsNotNull(result.Value, "Response value should not be null");

			dynamic response = result.Value;
			Assert.AreEqual(expectedUrl, response.Url, "The response URL should match the request URL");

			dynamic headers = response.Headers;
			Assert.IsNotNull(headers, "Headers should not be null");
			Assert.AreEqual("httpbin.org", headers.Host, "Host header should be 'httpbin.org'");
		}

		private void AssertImageResponse(Result<byte[]> result, string imgType)
		{
			Assert.AreEqual(200, result.StatusCode, "Status code should be 200");
			Assert.IsNotNull(result.Value, "Response value should not be null");
			Assert.IsTrue(result.Value.Length > 0, "Image data should not be empty");
			File.WriteAllBytes($"test_image.{imgType}", result.Value);
		}
	}
}
