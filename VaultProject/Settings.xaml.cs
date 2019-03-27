﻿using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace VaultProject
{
  public partial class Settings : Window
  {

    private App.D_IsSetupOver _delegate;
    public Settings(App.D_IsSetupOver sender)
    {
      InitializeComponent();
      Settings_DataPath.Text = R.Get("DataPath");
      Settings_FileName.Text = R.Get("FileName");
      Settings_PasswordBox.Password = Crypto.Decrypt(R.Get("Password"), CryptoMode.Password);
      Settings_AutoRunButton.Content = R.IsAutoRunSet() ? "UNSET" : "SET";
      _delegate = sender;
    }

    private void Settings_ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
      if (Settings_DataPath.Text != "")
      {
        R.Set("DataPath", Settings_DataPath.Text);
        R.Set("FileName", Settings_FileName.Text);
      }
      R.Set("Password", Crypto.Encrypt(Settings_PasswordBox.Password, CryptoMode.Password));
      Close();
    }

    private void Settings_OpenDataPath_Click(object sender, RoutedEventArgs e)
    {
      CommonOpenFileDialog dlg = new CommonOpenFileDialog
      {
        Title = "Choose folder",
        IsFolderPicker = true,
        EnsureFileExists = true,
        EnsurePathExists = true,
        EnsureReadOnly = false,
        EnsureValidNames = true,
        Multiselect = false,
        ShowPlacesList = true
      };
      if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
      {
        string folder = dlg.FileName;
        Settings_DataPath.Text = folder;
      }
    }

    private void Settings_ButtonClick(object sender, RoutedEventArgs e)
    {
      if (sender == Settings_AutoRunButton)
      {
        if (R.IsAutoRunSet())
        {
          R.UnsetAutoRun();
          Settings_AutoRunButton.Content = "SET";
        }
        else
        {
          R.SetAutoRun();
          Settings_AutoRunButton.Content = "UNSET";
        }
      }
      if (sender == Settings_SecureWordButton)
      {
        string newSecureWord = UsefulFunctions.GetUniqueKey(32);
        R.Set("SecureWord", newSecureWord);
        Vault.RewriteFile();
      }
      if (sender == Settings_EraseButton)
      {
        Vault.EraseData();
      }
      if (sender == Settings_OpenDir)
      {
        System.Diagnostics.Process.Start(R.Get("DataPath"));
      }
    }

    private void Settings_CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      _delegate(true);
    }
  }
}

