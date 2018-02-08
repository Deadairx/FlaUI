﻿using System;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using FlaUI.Core.UITests.TestFramework;
using NUnit.Framework;

namespace FlaUI.Core.UITests.Elements
{
    [TestFixture(AutomationType.UIA2, TestApplicationType.WinForms)]
    [TestFixture(AutomationType.UIA2, TestApplicationType.Wpf)]
    [TestFixture(AutomationType.UIA3, TestApplicationType.WinForms)]
    [TestFixture(AutomationType.UIA3, TestApplicationType.Wpf)]
    public class ComboBoxTests : UITestBase
    {
        private Window _mainWindow;

        public ComboBoxTests(AutomationType automationType, TestApplicationType appType)
            : base(automationType, appType)
        {
        }

        [OneTimeSetUp]
        public void TestOneTimeSetup()
        {
            _mainWindow = App.GetMainWindow(Automation);
            Assert.That(_mainWindow, Is.Not.Null);
        }

        [Test]
        [TestCase("EditableCombo")]
        [TestCase("NonEditableCombo")]
        public void SelectedItemTest(string comboBoxId)
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(comboBoxId)).AsComboBox();
            combo.Items[1].Select();
            var selectedItem = combo.SelectedItem;
            Assert.That(selectedItem, Is.Not.Null);
            Assert.That(selectedItem.Text, Is.EqualTo("Item 2"));
        }

        [Test]
        [TestCase("EditableCombo")]
        [TestCase("NonEditableCombo")]
        public void SelectByIndexTest(string comboBoxId)
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(comboBoxId)).AsComboBox();
            combo.Select(1);
            var selectedItem = combo.SelectedItem;
            Assert.That(selectedItem, Is.Not.Null);
            Assert.That(selectedItem.Text, Is.EqualTo("Item 2"));
        }

        [Test]
        [TestCase("EditableCombo")]
        [TestCase("NonEditableCombo")]
        public void SelectByTextTest(string comboBoxId)
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(comboBoxId)).AsComboBox();
            combo.Select("Item 2");
            var selectedItem = combo.SelectedItem;
            Assert.That(selectedItem, Is.Not.Null);
            Assert.That(selectedItem.Text, Is.EqualTo("Item 2"));
        }

        [Test]
        [TestCase("EditableCombo")]
        [TestCase("NonEditableCombo")]
        public void ExpandCollapseTest(string comboBoxId)
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId(comboBoxId)).AsComboBox();
            combo.Expand();
            Assert.That(combo.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            combo.Collapse();
            Assert.That(combo.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Collapsed));
        }

        [Test]
        public void EditableTextTest()
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("EditableCombo")).AsComboBox();
            combo.EditableText = "Item 3";
            Assert.That(combo.SelectedItem, Is.Not.Null);
            Assert.That(combo.SelectedItem.Text, Is.EqualTo("Item 3"));
        }

        [Test]
        public void AssertMessageBoxCanBeRetrievedInSelection()
        {
            var combo = _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("NonEditableCombo")).AsComboBox();
            combo.Expand();
            // Wait for the windows animation
            System.Threading.Thread.Sleep(1000);
            combo.Items[3].Click();
            var window = Retry.While(() => _mainWindow.FindFirstDescendant(cf => cf.ByClassName("#32770"))?.AsWindow(), w => w == null, TimeSpan.FromMilliseconds(1000));
            Assert.That(window, Is.Not.Null, "Expected a window that was shown when combobox item was selected");
            window.FindFirstDescendant(cf => cf.ByAutomationId("Close")).AsButton().Invoke();
        }
    }
}
