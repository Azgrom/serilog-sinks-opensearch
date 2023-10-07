namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var httpLocalhost = "https://mail.proton.me/u/0/inbox/t5aA72-SAJyg6A8A1646IeQec6nw6AEv4d7_n1jevAjRIip3pWzORiz7OwJtehY-MEZi6Y790dAGN_HvuWpu3Q==";
        var uri = new Uri(httpLocalhost);

        var checkSchemeName = Uri.CheckSchemeName(uri.Scheme);
        var uri1 = new UriBuilder(httpLocalhost)
        {
            Password = "changeme",
            UserName = "elastic"
        }.Uri;
        var uriSchemeHttp = Uri.UriSchemeHttp;
        Console.WriteLine();
    }
}
