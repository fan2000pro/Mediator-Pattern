using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public string Name { get; private set; }
    private ChatMediator _mediator;
    public List<string> MessageHistory { get; private set; } = new List<string>();

    public User(string name, ChatMediator mediator)
    {
        Name = name;
        _mediator = mediator;
    }

    public void SendMessage(string recipientName, string message)
    {
        _mediator.SendMessage(this, recipientName, message);
    }

    public void ReceiveMessage(string senderName, string message)
    {
        string formattedMessage = $"{senderName}: {message}";
        MessageHistory.Add(formattedMessage);
        Console.WriteLine($"[{Name}] Received: {formattedMessage}");
    }
}

public class ChatMediator
{
    private List<User> _users = new List<User>();

    public void AddUser(User user)
    {
        if (!_users.Contains(user))
        {
            _users.Add(user);
            Console.WriteLine($"User {user.Name} joined the chat.");
        }
    }

    public void RemoveUser(User user)
    {
        if (_users.Contains(user))
        {
            _users.Remove(user);
            Console.WriteLine($"User {user.Name} left the chat.");
        }
    }

    public void SendMessage(User sender, string recipientName, string message)
    {
        User recipient = _users.FirstOrDefault(u => u.Name == recipientName);

        if (recipient != null)
        {
            recipient.ReceiveMessage(sender.Name, message);
        }
        else
        {
            Console.WriteLine($"[{sender.Name}] Error: User {recipientName} not found.");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        ChatMediator mediator = new ChatMediator();

        User user1 = new User("Alice", mediator);
        User user2 = new User("Bob", mediator);
        User user3 = new User("Charlie", mediator);

        mediator.AddUser(user1);
        mediator.AddUser(user2);
        mediator.AddUser(user3);

        user1.SendMessage("Bob", "Hello Bob!");
        user2.SendMessage("Alice", "Hi Alice!");
        user3.SendMessage("Alice", "Good morning, Alice!");
        user3.SendMessage("Unknown", "This message should fail.");

        mediator.RemoveUser(user2);
        user1.SendMessage("Bob", "Are you still here?");

        Console.WriteLine("\nMessage History:");
        Console.WriteLine($"Alice: {string.Join(", ", user1.MessageHistory)}");
        Console.WriteLine($"Bob: {string.Join(", ", user2.MessageHistory)}");
        Console.WriteLine($"Charlie: {string.Join(", ", user3.MessageHistory)}");
    }
}
