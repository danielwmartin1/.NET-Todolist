using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Controllers;
using TodoListApp.Models;
using System.Collections.Generic;

namespace TodoListApp.Tests
{
    public class TodoControllerTests
    {
        [Fact]
        public void Index_ReturnsViewWithMockedTodoItems()
        {
            // Arrange
            var mockTodoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Test 2", IsCompleted = true }
            };

            var mockController = new Mock<TodoController>();
            mockController.Setup(c => c.Index()).Returns(new ViewResult { Model = mockTodoItems });

            // Act
            var result = mockController.Object.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as List<TodoItem>;
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Add_ValidTodoItem_RedirectsToIndex()
        {
            // Arrange
            var controller = new TodoController();
            var newItem = new TodoItem { Title = "Test Todo", IsCompleted = false };

            // Act
            var result = controller.Add(newItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Delete_ValidId_RedirectsToIndex()
        {
            // Arrange
            var controller = new TodoController();
            var validId = 1;

            // Act
            var result = controller.Delete(validId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
