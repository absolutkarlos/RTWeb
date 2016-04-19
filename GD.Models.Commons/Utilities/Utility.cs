using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace GD.Models.Commons.Utilities
{
	public class Utility
	{
		public struct Coordenates
		{
			public int Degress;
			public int Minutes;
			public decimal Seconds;
			public string Geo; // N,S,E,W
		}

		public static string ConvertToDegressMinutesSeconds(double position, bool isLong)
		{
			Coordenates ret = new Coordenates();

			// Negative: North
			// Positive: South
			// if isLong = true
			// Negative: East
			// Positive: West
			double absValue = Math.Abs(Math.Round(position * 1000000));
			int sign = Math.Sign(position);

			ret.Degress = (int)Math.Floor(absValue / 1000000);
			ret.Minutes = (int)Math.Floor(((absValue / 1000000) - Math.Floor(absValue / 1000000)) * 60);
			ret.Seconds = (Decimal)Math.Floor(((((absValue / 1000000) - Math.Floor(absValue / 1000000)) * 60) - Math.Floor(((absValue / 1000000) - Math.Floor(absValue / 1000000)) * 60)) * 100000) * 60 / 100000;

			if (isLong)
				ret.Geo = sign > 0 ? @"W" : @"E";
			else
				if (sign > 0)
					ret.Geo = @"N";
			else
				ret.Geo = @"S";

			return ret.Geo + @" " + ret.Degress + @"º " + ret.Minutes + @"' " + ret.Seconds + '"';
		}

		public static string RestClient(string url, string contentType)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = contentType;
			string result;
			using (var response = (HttpWebResponse)request.GetResponse())
			{
				var reader = new StreamReader(response.GetResponseStream());
				result = reader.ReadToEnd();
			}
			return result;
		}

		public static T MapperQueryString<T>(string queryString) where T : new()
		{
			var model = new T();
			var parseQueryString = HttpUtility.ParseQueryString(queryString);
			(model.GetType().GetProperties()).ToList().ForEach(p => p.SetValue(model, parseQueryString[p.Name]));
			return model;
		}

		private string Key = "";
		private readonly byte[] IVector = new byte[8] { 27, 9, 45, 27, 0, 72, 171, 54 };

		public string Encrypt(string inputString)
		{

			byte[] buffer = Encoding.ASCII.GetBytes(inputString);
			TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			tripleDes.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));
			tripleDes.IV = IVector;
			ICryptoTransform ITransform = tripleDes.CreateEncryptor();
			return Convert.ToBase64String(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
		}

		public string Decrypt(string inputString)
		{
			byte[] buffer = Convert.FromBase64String(inputString);
			TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			tripleDes.Key = MD5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(Key));
			tripleDes.IV = IVector;
			ICryptoTransform ITransform = tripleDes.CreateDecryptor();
			return Encoding.UTF8.GetString(ITransform.TransformFinalBlock(buffer, 0, buffer.Length));
		}

		public string GetSHA1HashData(string value)
		{
			var data = Encoding.UTF8.GetBytes(value);
			var dataprueba = Encoding.UTF8.GetChars(data);
			var hashData = new SHA1Managed().ComputeHash(data);

			var hash = string.Empty;

			foreach (var b in hashData)
				hash += b.ToString("X2");

			return hash;
		}

		/// <summary>
		/// Reads the content of a txt file
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public string readTextFile(string filePath)
		{
			// Read the file as one string. 
			string text = System.IO.File.ReadAllText(filePath);

			return text;
		}

		/// <summary>
		/// Reads a word document and returns its content.
		/// </summary>
		/// <param name="filePath">The server absolute path, i.e. Server.MapPath(Url.Content("~/App_Data/Vzla_Terms_of_Use_EducaTablet v1_4_09_12_esp.doc"))</param>
		/// <returns></returns>
		public string readWordDocument(string filePath)
		{
			var word = new Microsoft.Office.Interop.Word.Application();

			object miss = Missing.Value;

			object path = filePath;
			object readOnly = true;
			Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
			string totaltext = "";
			for (int i = 0; i < docs.Paragraphs.Count; i++)
			{
				totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text;
			}
			Console.WriteLine(totaltext);
			docs.Close();
			word.Quit();

			return totaltext;
		}

		/// <summary>
		/// Sends an SSL mail
		/// </summary>
		/// <param name="destiny"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public bool sendEmail(string destiny, string subject, string message)
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.To.Add(destiny);
				mail.Subject = subject;
				mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTP_FROM"]);
				mail.Body = message;
				mail.BodyEncoding = UTF8Encoding.UTF8;
				mail.IsBodyHtml = true;

				SmtpClient smtp = new SmtpClient();
				smtp.Timeout = 20000;
				smtp.EnableSsl = true;
				smtp.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
				smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTP_USER"], ConfigurationManager.AppSettings["SMTP_PASS"]);

				smtp.Send(mail);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Calls Rest POST Web Service, sending an object which is converted
		/// to json and then return the json from response or null if the 
		/// call was not OK
		/// </summary>
		/// <param name="obj">object that will be serialized into json to send</param>
		/// <param name="fullPath">the full url path to call the WS</param>
		/// <returns></returns>
		public string postJsonWs(object obj, string fullPath)
		{
			// Gets the url of the webservice
			string url = fullPath;

			// Create a request using a URL that can receive a post. 
			WebRequest request = WebRequest.Create(url);
			// Set the Method property of the request to POST.
			request.Method = "POST";
			// Convert the object into a json string
			string json = JsonConvert.SerializeObject(obj);
			// Convert the POST data to byte array
			byte[] byteArray = Encoding.UTF8.GetBytes(json);
			// Set the ContentType property of the WebRequest.
			request.ContentType = "application/json";
			// Add headers that WS is requiring
			request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
			request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,es;q=0.6,gl;q=0.4");
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = byteArray.Length;
			try
			{
				// Get the request stream.
				Stream dataStream = request.GetRequestStream();
				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);
				// Close the Stream object.
				dataStream.Close();
				// Get the response.
				WebResponse response = request.GetResponse();

				string responseFromServer = null;

				if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
				{
					//// Display the status.
					//Console.WriteLine(((HttpWebResponse) response).StatusDescription);
					// Get the stream containing content returned by the server.
					dataStream = response.GetResponseStream();
					// Open the stream using a StreamReader for easy access.
					StreamReader reader = new StreamReader(dataStream);
					// Read the content.
					responseFromServer = reader.ReadToEnd();
					//close the response stream
					reader.Close();
				}
				// Clean up the streams.
				dataStream.Close();
				response.Close();
				return responseFromServer;

			}
			catch (Exception)
			{

				return null;
			}
		}

		/// <summary>
		/// Calls Rest POST Web Service, sending an object which is converted
		/// to json and then return the json from response or null if the 
		/// call was not OK
		/// </summary>
		/// <param name="payload">the raw json string to send to WS</param>
		/// <param name="fullPath">the full url path to call the WS</param>
		/// <param name="contenttype"></param>
		/// <returns></returns>
		public string postWs(string payload, string fullPath, string contenttype = "application/json")
		{
			// Gets the url of the webservice
			string url = fullPath;

			// Create a request using a URL that can receive a post. 
			WebRequest request = WebRequest.Create(url);
			// Set the Method property of the request to POST.
			request.Method = "POST";
			// Convert the POST data to byte array
			byte[] byteArray = Encoding.UTF8.GetBytes(payload);
			// Set the ContentType property of the WebRequest.
			request.ContentType = contenttype;
			// Add headers that WS is requiring
			request.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
			request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,es;q=0.6,gl;q=0.4");
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = byteArray.Length;
			// Get the request stream.
			Stream dataStream = request.GetRequestStream();
			// Write the data to the request stream.
			dataStream.Write(byteArray, 0, byteArray.Length);
			// Close the Stream object.
			dataStream.Close();
			// Get the response.
			WebResponse response = request.GetResponse();

			string responseFromServer = null;

			if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
			{
				//// Display the status.
				//Console.WriteLine(((HttpWebResponse) response).StatusDescription);
				// Get the stream containing content returned by the server.
				dataStream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.
				responseFromServer = reader.ReadToEnd();
				//close the response stream
				reader.Close();
			}

			// Clean up the streams.
			dataStream.Close();
			response.Close();

			return responseFromServer;
		}

		/// <summary>
		/// Get a random password for an specified length
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public string getRandomPassword(int length)
		{
			string pw = null;

			RandomPassword objPassword = new RandomPassword();

			try
			{
				pw = objPassword.Generate(length);
			}
			catch (Exception)
			{
				return null;
			}
			return pw;
		}
	}

	public class RandomPassword
	{
		// Define default min and max password lengths.
		private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
		private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

		// Define supported password characters divided into groups.
		// You can add (or remove) characters to (from) these groups.
		private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
		private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
		private static string PASSWORD_CHARS_NUMERIC = "23456789";
		private static string PASSWORD_CHARS_SPECIAL = "_@";

		/// <summary>
		/// Generates a random password.
		/// </summary>
		/// <returns>
		/// Randomly generated password.
		/// </returns>
		/// <remarks>
		/// The length of the generated password will be determined at
		/// random. It will be no shorter than the minimum default and
		/// no longer than maximum default.
		/// </remarks>
		public string Generate()
		{
			return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
							DEFAULT_MAX_PASSWORD_LENGTH);
		}

		/// <summary>
		/// Generates a random password of the exact length.
		/// </summary>
		/// <param name="length">
		/// Exact password length.
		/// </param>
		/// <returns>
		/// Randomly generated password.
		/// </returns>
		public string Generate(int length)
		{
			return Generate(length, length);
		}

		/// <summary>
		/// Generates a random password.
		/// </summary>
		/// <param name="minLength">
		/// Minimum password length.
		/// </param>
		/// <param name="maxLength">
		/// Maximum password length.
		/// </param>
		/// <returns>
		/// Randomly generated password.
		/// </returns>
		/// <remarks>
		/// The length of the generated password will be determined at
		/// random and it will fall with the range determined by the
		/// function parameters.
		/// </remarks>
		public string Generate(int minLength,
									  int maxLength)
		{
			// Make sure that input parameters are valid.
			if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
				return null;

			// Create a local array containing supported password characters
			// grouped by types. You can remove character groups from this
			// array, but doing so will weaken the password strength.
			char[][] charGroups = new char[][] 
        {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

			// Use this array to track the number of unused characters in each
			// character group.
			int[] charsLeftInGroup = new int[charGroups.Length];

			// Initially, all characters in each group are not used.
			for (int i = 0; i < charsLeftInGroup.Length; i++)
				charsLeftInGroup[i] = charGroups[i].Length;

			// Use this array to track (iterate through) unused character groups.
			int[] leftGroupsOrder = new int[charGroups.Length];

			// Initially, all character groups are not used.
			for (int i = 0; i < leftGroupsOrder.Length; i++)
				leftGroupsOrder[i] = i;

			// Because we cannot use the default randomizer, which is based on the
			// current time (it will produce the same "random" number within a
			// second), we will use a random number generator to seed the
			// randomizer.

			// Use a 4-byte array to fill it with random bytes and convert it then
			// to an integer requestObj.
			byte[] randomBytes = new byte[4];

			// Generate 4 random bytes.
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			// Convert 4 bytes into a 32-bit integer requestObj.
			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1] << 16 |
						randomBytes[2] << 8 |
						randomBytes[3];

			// Now, this is real randomization.
			Random random = new Random(seed);

			// This array will hold password characters.
			char[] password = null;

			// Allocate appropriate memory for the password.
			if (minLength < maxLength)
				password = new char[random.Next(minLength, maxLength + 1)];
			else
				password = new char[minLength];

			// Index of the next character to be added to password.
			int nextCharIdx;

			// Index of the next character group to be processed.
			int nextGroupIdx;

			// Index which will be used to track not processed character groups.
			int nextLeftGroupsOrderIdx;

			// Index of the last non-processed character in a group.
			int lastCharIdx;

			// Index of the last non-processed group.
			int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

			// Generate password characters one at a time.
			for (int i = 0; i < password.Length; i++)
			{
				// If only one character group remained unprocessed, process it;
				// otherwise, pick a random character group from the unprocessed
				// group list. To allow a special character to appear in the
				// first position, increment the second parameter of the Next
				// function call by one, i.e. lastLeftGroupsOrderIdx + 1.
				if (lastLeftGroupsOrderIdx == 0)
					nextLeftGroupsOrderIdx = 0;
				else
					nextLeftGroupsOrderIdx = random.Next(0,
														 lastLeftGroupsOrderIdx);

				// Get the actual index of the character group, from which we will
				// pick the next character.
				nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

				// Get the index of the last unprocessed characters in this group.
				lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

				// If only one unprocessed character is left, pick it; otherwise,
				// get a random character from the unused character list.
				if (lastCharIdx == 0)
					nextCharIdx = 0;
				else
					nextCharIdx = random.Next(0, lastCharIdx + 1);

				// Add this character to the password.
				password[i] = charGroups[nextGroupIdx][nextCharIdx];

				// If we processed the last character in this group, start over.
				if (lastCharIdx == 0)
					charsLeftInGroup[nextGroupIdx] =
											  charGroups[nextGroupIdx].Length;
				// There are more unprocessed characters left.
				else
				{
					// Swap processed character with the last unprocessed character
					// so that we don't pick it until we process all characters in
					// this group.
					if (lastCharIdx != nextCharIdx)
					{
						char temp = charGroups[nextGroupIdx][lastCharIdx];
						charGroups[nextGroupIdx][lastCharIdx] =
									charGroups[nextGroupIdx][nextCharIdx];
						charGroups[nextGroupIdx][nextCharIdx] = temp;
					}
					// Decrement the number of unprocessed characters in
					// this group.
					charsLeftInGroup[nextGroupIdx]--;
				}

				// If we processed the last group, start all over.
				if (lastLeftGroupsOrderIdx == 0)
					lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
				// There are more unprocessed groups left.
				else
				{
					// Swap processed group with the last unprocessed group
					// so that we don't pick it until we process all groups.
					if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
					{
						int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
						leftGroupsOrder[lastLeftGroupsOrderIdx] =
									leftGroupsOrder[nextLeftGroupsOrderIdx];
						leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
					}
					// Decrement the number of unprocessed groups.
					lastLeftGroupsOrderIdx--;
				}
			}

			// Convert password characters into a string and return the result.
			return new string(password);
		}
	}

	public static class StringMethodExtensions
	{
		private static string _paraBreak = "\r\n\r\n";
		private static string _link = "<a href=\"{0}\">{1}</a>";
		private static string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

		/// <summary>
		/// Returns a copy of this string converted to HTML markup.
		/// </summary>
		public static string ToHtml(this string s)
		{
			return ToHtml(s, false);
		}

		/// <summary>
		/// Returns a copy of this string converted to HTML markup.
		/// </summary>
		/// <param name="nofollow">If true, links are given "nofollow"
		/// attribute</param>
		public static string ToHtml(this string s, bool nofollow)
		{
			StringBuilder sb = new StringBuilder();

			int pos = 0;
			while (pos < s.Length)
			{
				// Extract next paragraph
				int start = pos;
				pos = s.IndexOf(_paraBreak, start);
				if (pos < 0)
					pos = s.Length;
				string para = s.Substring(start, pos - start).Trim();

				// Encode non-empty paragraph
				if (para.Length > 0)
					EncodeParagraph(para, sb, nofollow);

				// Skip over paragraph break
				pos += _paraBreak.Length;
			}
			// Return result
			return sb.ToString();
		}

		/// <summary>
		/// Encodes a single paragraph to HTML.
		/// </summary>
		/// <param name="s">Text to encode</param>
		/// <param name="sb">StringBuilder to write results</param>
		/// <param name="nofollow">If true, links are given "nofollow"
		/// attribute</param>
		private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow)
		{
			// Start new paragraph
			sb.AppendLine("<p>");

			// HTML encode text
			s = HttpUtility.HtmlEncode(s);

			// Convert single newlines to <br>
			s = s.Replace(Environment.NewLine, "<br />\r\n");

			// Encode any hyperlinks
			EncodeLinks(s, sb, nofollow);

			// Close paragraph
			sb.AppendLine("\r\n</p>");
		}

		/// <summary>
		/// Encodes [[URL]] and [[Text][URL]] links to HTML.
		/// </summary>
		/// <param name="text">Text to encode</param>
		/// <param name="sb">StringBuilder to write results</param>
		/// <param name="nofollow">If true, links are given "nofollow"
		/// attribute</param>
		private static void EncodeLinks(string s, StringBuilder sb, bool nofollow)
		{
			// Parse and encode any hyperlinks
			int pos = 0;
			while (pos < s.Length)
			{
				// Look for next link
				int start = pos;
				pos = s.IndexOf("[[", pos);
				if (pos < 0)
					pos = s.Length;
				// Copy text before link
				sb.Append(s.Substring(start, pos - start));
				if (pos < s.Length)
				{
					string label, link;

					start = pos + 2;
					pos = s.IndexOf("]]", start);
					if (pos < 0)
						pos = s.Length;
					label = s.Substring(start, pos - start);
					int i = label.IndexOf("][");
					if (i >= 0)
					{
						link = label.Substring(i + 2);
						label = label.Substring(0, i);
					}
					else
					{
						link = label;
					}
					// Append link
					sb.Append(String.Format(nofollow ? _linkNoFollow : _link, link, label));

					// Skip over closing "]]"
					pos += 2;
				}
			}
		}
	}
}