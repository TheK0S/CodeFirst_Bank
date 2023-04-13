using CodeFirst;


int input;
bool AppIsOn = true;
List<User> users = GetListFromDBTable() ?? new List<User>();

while (AppIsOn)
{
    while (true)
    {
        Console.WriteLine("\n\tСделайте выбор\n" +
        "1 - Вывести список пользователей из базы\n" +
        "2 - Добавить пользователя\n" +
        "3 - Удалить пользователя\n" +
        "4 - Изменить баланс пользователя\n" +
        "5 - Добавить 5 тестовых пользователей для проверки программы (Даже не выбрать повторно)\n" +
        "0 - Выход");

        if (int.TryParse(Console.ReadLine(), out input) && input >= 0 && input <= 5)
            break;
        else
            Console.WriteLine("Ошибка ввода");
    }

    if(input == 5)
    {
        List<User> testList = new List<User>
        {
            new User(1, "Василий", "Пупкин", "Иванович", 33, "Клиент", "UA123456789012345678901234567", 330000),
            new User(2, "Петр", "Бубкин", "Степанович", 33, "Клиент", "UA123456789012345678901234568", 253000),
            new User(3, "John", "Wick", null, 33, "Клиент", "UA123456789012345678901234569", 500000)
        };

        WriteToDBTable(testList);
    }

    switch ((MainMenu)input)
    {
        case MainMenu.Exit:
            AppIsOn = false;
            break;


        case MainMenu.ExportUsers:
            PrintList(GetListFromDBTable());
            break;


        case MainMenu.AddUser:
            Console.WriteLine("\nВведите Имя: ");
            string firstName = Console.ReadLine() ?? "no first_name";

            Console.WriteLine("Введите Фамилию: ");
            string lastName = Console.ReadLine() ?? "no last_name";

            Console.WriteLine("Введите Отчество: ");
            string patronomic = Console.ReadLine() ?? "no patronomic";

            int age;
            while (true)
            {
                Console.WriteLine("\nВведите Возраст: ");
                if (int.TryParse(Console.ReadLine(), out age))
                    break;
                else
                    Console.WriteLine("Ошибка ввода");
            }

            Console.WriteLine("Введите Роль пользователя (Клиент, сотрудник и т.д.): ");
            string role = Console.ReadLine() ?? "no role";

            Console.WriteLine("Введите IBAN: ");
            string account = Console.ReadLine() ?? "no patronomic";

            decimal balance;
            while (true)
            {
                Console.WriteLine("\nВведите Баланс на счете: ");
                if (decimal.TryParse(Console.ReadLine(), out balance))
                    break;
                else
                    Console.WriteLine("Ошибка ввода");
            }

            if (AddUserToDBTable(new User(GetBiggestId() + 1, firstName, lastName, patronomic, age, role, account, balance)))
            {
                users = GetListFromDBTable();
                Console.WriteLine("Пользователь добавлен");
            }                
            else
                Console.WriteLine("Ошибка при добавлении, пользователь не добавлен в базу");
            break;


        case MainMenu.RemoveUser:
            while (true)
            {
                Console.WriteLine("Введите Id пользователя для удаления из базы");
                if (int.TryParse(Console.ReadLine(), out input))
                    break;
                else
                    Console.WriteLine("Ошибка ввода");
            }

            if (RemoveUserFromDBTable(input))
            {
                users = GetListFromDBTable();
                Console.WriteLine("Пользователь удален");
            }                
            else
                Console.WriteLine("Ошибка при удалении, пользователь не удален");
            break;


        case MainMenu.ChangeBalance:
            Console.WriteLine("Введите Id пользователя, баланс которого хотите изменить");
            while (true)
            {
                Console.WriteLine("Введите Id пользователя, баланс которого хотите изменить");
                if (int.TryParse(Console.ReadLine(), out input))
                    break;
                else
                    Console.WriteLine("Ошибка ввода");
            }
            string userToString = GetUserToString(input, users);
            Console.WriteLine(userToString);
            if (userToString[0] == 'I')
            {
                while (true)
                {
                    Console.WriteLine("Введите новую сумму");
                    if (decimal.TryParse(Console.ReadLine(), out balance))
                        break;
                    else
                        Console.WriteLine("Ошибка ввода");
                }
                                
                Console.WriteLine(ChangeBalance(input, balance, users));
                users = GetListFromDBTable();
            }         
                       
            break;


        default:
            break;
    }

}

static string GetUserToString(int Id, List<User> users)
{
    foreach (var item in users)
    {
        if(item.Id == Id)
        {
            return $"Id: {item.Id}\n" +
            $"Имя: {item.FirstName ?? "no_first_name"} {item.LastName ?? "no_last_name"} {item.PatronomicName ?? "no_patronomic"}\n" +
            $"Возраст: {item.Age}\n" +
            $"Роль: {item.Role ?? "no_role"}\n" +
            $"Аккаунт: {item.Account}\n" +
            $"Баланс: {item.Balance}\n\n";
        }
    }

    return $"Пользователь с Id = {Id} не найден";
}

static string ChangeBalance(int Id, decimal balance, List<User> users)
{
    foreach (var item in users)
    {
        if(item.Id == Id)
        {
            item.Balance = balance;
            return $"Баланс обновлен. Новый баланс: {balance}";
        }
    }
    return $"Пользователь с Id = {Id} не найден";
}

static int GetBiggestId()
{
    int id = 0;
    try
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            foreach (var item in db.Users)
            {
                if(item.Id > id)
                    id = item.Id;
            }
        }
        return id;
    }
    catch (Exception)
    {
        return 0;
    }
}

static bool RemoveUserFromDBTable(int Id)
{
    try
    {
        bool isRemoved = false;
        using (ApplicationContext db = new ApplicationContext())
        {
            foreach (var item in db.Users)
            {
                if(item.Id == Id)
                {
                    db.Users.Remove(item);
                    db.SaveChanges();
                    isRemoved = true;
                }
            }
        }
        return isRemoved;
    }
    catch (Exception)
    {
        return false;
    }
}

static bool AddUserToDBTable(User user)
{
    try
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        return true;
    }
    catch (Exception)
    {
        return false;
    }
}

static string WriteToDBTable(List<User> users)
{
    try
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            foreach (var user in users)
                db.Users.Add(user);

            db.SaveChanges();
        }

        return $"В базу данных дабавлено {users.Count} записей";
    }
    catch (Exception ex)
    {
        return ex.Message;
    }
    
}
static void PrintList(List<User> users)
{
    foreach (var item in users)
    {
        Console.WriteLine($"Id: {item.Id}\n" +
            $"Имя: {item.FirstName ?? "no_first_name"} {item.LastName ?? "no_last_name"} {item.PatronomicName ?? "no_patronomic"}\n" +
            $"Возраст: {item.Age}\n" +
            $"Роль: {item.Role ?? "no_role"}\n" +
            $"Аккаунт: {item.Account}\n" +
            $"Баланс: {item.Balance}\n\n");
    }
}
static List<User> GetListFromDBTable()
{
    try
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Users.ToList();
        }
    }
    catch (Exception)
    {
        return new List<User>();
    }
}

enum MainMenu { Exit, ExportUsers, AddUser, RemoveUser, ChangeBalance }