using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;

namespace TodoListApp.TodoListApp.Tests
{
  public class Tests
  {
    private TodoList? _todoList;

    [SetUp]
    public void Setup()
    {
      _todoList = new TodoList();
    }

    [Test]
    public void AddItem_ShouldAddItemToList()
    {
      // Arrange
      var item = "Buy groceries";

      // Act
      if (_todoList == null) throw new InvalidOperationException("TodoList is not initialized.");

      _todoList.AddItem(item);

      // Assert
      Assert.That(_todoList.GetItems(), Does.Contain(item));
    }

    [Test]
    public void RemoveItem_ShouldRemoveItemFromList()
    {
      // Arrange
      var item = "Buy groceries";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
      _todoList.AddItem(item);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

      // Act
      _todoList.RemoveItem(item);

      // Assert
      Assert.That(_todoList.GetItems().Contains(item), Is.False);
    }

    [Test]
    public void GetItems_ShouldReturnAllItems()
    {
      // Arrange
      var item1 = "Buy groceries";
      var item2 = "Walk the dog";
      if (_todoList == null) throw new InvalidOperationException("TodoList is not initialized.");
      _todoList.AddItem(item1);
      _todoList.AddItem(item2);

      // Act
      var items = _todoList.GetItems();

      // Assert
      Assert.That(items.Count, Is.EqualTo(2));
      Assert.That(items, Does.Contain(item1));
      Assert.That(items, Does.Contain(item2));
    }
  }

  // Example TodoList class for reference
  public class TodoList
  {
    private readonly List<string> _items = new();

    public void AddItem(string item) => _items.Add(item);

    public void RemoveItem(string item) => _items.Remove(item);

    public List<string> GetItems() => new List<string>(_items);
  }
}
