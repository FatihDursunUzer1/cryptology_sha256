

using System.Security.Cryptography;
using System.Text;

class Program
{
    

   public static async Task AddUser()
    {
        (var userName,var password)=HomePage();
        var checkLogin=await CheckLogin(userName, password, true);
        if(!checkLogin)
        await HashPassword(userName,password,true);
        Console.WriteLine("Hesabınız olusturulmustur, giris ekranina yonlendiriliyorsunuz");
    }

    private static (string,string) HomePage()
    {
        Console.WriteLine("Kullanıcı adını giriniz");
        var userName = Console.ReadLine();
        Console.WriteLine("Sifreyi giriniz");
        var password = Console.ReadLine();
        return (userName, password);
    }

    public async static Task<string> HashPassword(string username,string password,bool isRegister=false)
    {
        try {
            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                if (isRegister)
                {
                    string[] texts = { username, builder.ToString() };
                    WriteToFile(texts.ToList());
                }
                return builder.ToString();
            }
        }
        catch(Exception ex)
        {
            var myex = ex;
            return null;
        }
    }

    public static async Task<bool> CheckLogin(string username, string password,bool isRegister=false)
    {
        FileInfo file = new FileInfo("/Users/fatihdursun.uzer/Projects/CheckHash/CheckHash/database.txt");
        var lines = File.ReadAllLines(file.FullName);
        var hashPassword = await HashPassword(username,password);
        var checkUser = false;
        for (int i = 0; i < lines.Length; ++i)
            if (lines[i] == username)

            {
                checkUser = true;
                if (!isRegister)
                {
                    bool isTruePassword = hashPassword == lines[i + 1];
                    
                    if (isTruePassword)
                        Console.WriteLine("Giriş yapabilirsiniz");
                    else
                        Console.WriteLine("Giriş yapamazsınız");
                }
            }
        if (!checkUser)
            Console.WriteLine("Sistemde kayıt bulunamadı");
        if (checkUser && isRegister)
            Console.WriteLine("Bu kullanıcı adı daha önceden alınmış");
        return checkUser;
    }

    public static void Login()
    {
        (var userName,var password)=HomePage();
        CheckLogin(userName, password);
        
    }

    public static async Task WriteToFile(List<string> texts)
    {
        FileInfo file = new FileInfo("/Users/fatihdursun.uzer/Projects/CheckHash/CheckHash/database.txt");
        file.Directory.Create();
        await File.AppendAllLinesAsync(file.FullName,texts);
    }

    public static void Main(string[] args)
    {
        int keyboardNumber = 0;
        while (keyboardNumber!=1 && keyboardNumber!=2)
        {
            Console.WriteLine("Giriş yapmak için 1'i, Kullanıcı kaydı oluşturmak için 2'yi tuşlayınız");
            keyboardNumber = int.Parse(Console.ReadLine());
        }

        if (keyboardNumber == 2)
            AddUser();
        else if (keyboardNumber == 1)
            Login();
            
    }
}
