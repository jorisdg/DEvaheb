object Form1: TForm1
  Left = 317
  Top = 212
  Width = 800
  Height = 600
  HorzScrollBar.Visible = False
  VertScrollBar.Visible = False
  Caption = 'BehaveS'
  Color = clAppWorkSpace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  Menu = MainMenu1
  OldCreateOrder = False
  Position = poScreenCenter
  WindowState = wsMaximized
  OnClose = FormClose
  OnCreate = FormCreate
  OnResize = FormResize
  PixelsPerInch = 96
  TextHeight = 13
  object sourcememo: TMemo
    Left = 0
    Top = 0
    Width = 793
    Height = 553
    ScrollBars = ssBoth
    TabOrder = 0
  end
  object OpenDialog1: TOpenDialog
    FileName = '*.ibi;*.txt'
    Filter = 
      'Compiled Script (*.ibi)|*.ibi|Source Text (*.txt)|*.txt|Scripts ' +
      '(*.ibi; *.txt)|*.ibi;*.txt|All Files (*.*)|*.*'
    FilterIndex = 3
    Title = 'Choose a scriptfile...'
    Left = 80
    Top = 424
  end
  object MainMenu1: TMainMenu
    Left = 424
    Top = 424
    object File1: TMenuItem
      Caption = '&File'
      object Open1: TMenuItem
        Caption = '&New'
        OnClick = Open1Click
      end
      object Open2: TMenuItem
        Caption = '&Open'
        OnClick = Open2Click
      end
      object Save1: TMenuItem
        Caption = '&Save'
        OnClick = Save1Click
      end
      object Saveas1: TMenuItem
        Caption = 'Save &as'
        OnClick = Saveas1Click
      end
      object N5: TMenuItem
        Caption = '-'
      end
      object Exit1: TMenuItem
        Caption = 'E&xit'
        OnClick = Exit1Click
      end
    end
    object Edit1: TMenuItem
      Caption = '&Edit'
      OnClick = Edit1Click
      object Undo1: TMenuItem
        Caption = '&Undo'
        OnClick = Undo1Click
      end
      object N3: TMenuItem
        Caption = '-'
      end
      object Cut1: TMenuItem
        Caption = 'C&ut'
        OnClick = Cut1Click
      end
      object Copy1: TMenuItem
        Caption = '&Copy'
        OnClick = Copy1Click
      end
      object Paste1: TMenuItem
        Caption = '&Paste'
        OnClick = Paste1Click
      end
      object SelectAll1: TMenuItem
        Caption = 'Se&lect All'
        OnClick = SelectAll1Click
      end
    end
    object Help1: TMenuItem
      Caption = '&Help'
      object Help2: TMenuItem
        Caption = 'H&elp'
        Enabled = False
      end
      object N2: TMenuItem
        Caption = '-'
      end
      object About1: TMenuItem
        Caption = 'A&bout...'
        OnClick = About1Click
      end
    end
  end
  object SaveDialog1: TSaveDialog
    DefaultExt = 'txt'
    FileName = '*.txt'
    Filter = 'Source Text (*.txt)|*.txt|All Files (*.*)|*.*'
    Options = [ofOverwritePrompt, ofHideReadOnly, ofEnableSizing]
    Title = 'Save source text as...'
    Left = 48
    Top = 424
  end
  object OpenDialog2: TOpenDialog
    FileName = '*.map'
    Filter = 'Map Files|*.map'
    Title = 'Choose a map file..'
    Left = 112
    Top = 424
  end
end
