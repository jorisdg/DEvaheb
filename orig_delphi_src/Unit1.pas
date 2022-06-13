unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, Menus, ExtCtrls, ToolWin, ComCtrls, Buttons;

type
  TForm1 = class(TForm)
    OpenDialog1: TOpenDialog;
    MainMenu1: TMainMenu;
    File1: TMenuItem;
    Open1: TMenuItem;
    Open2: TMenuItem;
    Save1: TMenuItem;
    Saveas1: TMenuItem;
    Exit1: TMenuItem;
    SaveDialog1: TSaveDialog;
    Help1: TMenuItem;
    Help2: TMenuItem;
    N2: TMenuItem;
    About1: TMenuItem;
    Edit1: TMenuItem;
    Cut1: TMenuItem;
    Copy1: TMenuItem;
    Paste1: TMenuItem;
    Undo1: TMenuItem;
    N3: TMenuItem;
    SelectAll1: TMenuItem;
    N5: TMenuItem;
    OpenDialog2: TOpenDialog;
    sourcememo: TMemo;
    procedure Open1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Open2Click(Sender: TObject);
    procedure Save1Click(Sender: TObject);
    procedure Saveas1Click(Sender: TObject);
    procedure Exit1Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure About1Click(Sender: TObject);
    procedure Edit1Click(Sender: TObject);
    procedure Undo1Click(Sender: TObject);
    procedure Cut1Click(Sender: TObject);
    procedure Copy1Click(Sender: TObject);
    procedure Paste1Click(Sender: TObject);
    procedure SelectAll1Click(Sender: TObject);
    procedure FormResize(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
Const title = 'DEvaheb v1.0';

Procedure Savenow;

Type ProjectType = record
                name : string;
                mapfile : string;
        end;
Type PMemo = ^TMemo;
     PForm = ^TForm;
Var project : projecttype;

implementation

uses Unit2;

Type FormListPtr = ^FormListDescr;
     FormListDescr = record
        prev, next : FormListPtr;
        memo : PMemo;
        form : PForm;
     end;
Var FormList : FormListPtr;

Type singlearr = Array[1..4] of byte;
        singleptr = ^singlearr;
Var f : File of byte;
    rs : single;
    rsptr : singleptr;
Const spaces : string = '';

Type ConstListPtr = ^ConstListDescr;
        ConstListDescr = RECORD
                prev, next : ConstListPtr;
                val : Single;
                name : string;
        end;
Var list : ConstListPtr;
        Xdiff, Ydiff : Integer;

{$R *.DFM}

{---------------SOURCE <=> SCHEMATIC-------------------}

{---------------END SOURCE <=> SCHEMATIC-------------------}


{---------------DECOMPILER-------------------}
Function RealValue : Single;
Var b, bh : byte;
 Begin
  Read(f,bh);Read(f,bh);Read(f,bh); {...}
  Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh);{4...}
  Read(f,rsptr[1]);Read(f,rsptr[2]);Read(f,rsptr[3]);Read(f,rsptr[4]);
  RealValue:=rs;
 End;

Function RealString : String;
Var b, bh : byte;
    s : string;
 Begin
  Read(f,bh);Read(f,bh);Read(f,bh); {...}
  Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh);{4...}
  Read(f,rsptr[1]);Read(f,rsptr[2]);Read(f,rsptr[3]);Read(f,rsptr[4]);
  Str(rs:0:3,s);
  Realstring:=s;
 End;

Function IntString : String;
Var b, bh : byte;
    s : string;
 Begin
  Read(f,bh);Read(f,bh);Read(f,bh); {...}
  Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh);{4...}
  Read(f,rsptr[1]);Read(f,rsptr[2]);Read(f,rsptr[3]);Read(f,rsptr[4]);
  Str(rs:0:0,s);
  Intstring:=s;
 End;

Function StringValue(decompile : boolean) : string;
Var s : string;
    b, bh : byte;
    t : integer;
 Begin
  s:='';
  if decompile then
   Begin
    s:='"';
    Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh); {x....}
    For t := 1 to b-1 do
     Begin
      Read(f,bh);
      s:=s+Chr(bh);
     End;
    Read(f,bh);
    s:=s+'"';
   End
  Else
   Begin
   End;
  stringvalue:=s;
 End;

Function String7Value(decompile : boolean) : string;
Var s : string;
    b, bh : byte;
    t : integer;
 Begin
  s:='';
  if decompile then
   Begin
    s:='';
    Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh); {x....}
    For t := 1 to b-1 do
     Begin
      Read(f,bh);
      s:=s+Chr(bh);
     End;
    Read(f,bh);
   End
  Else
   Begin
   End;
  string7value:=s;
 End;

Function ConstValue(r : single) : string;
Var node : ConstListPtr;
    s : string;
 Begin
  node:=list;
  s:='';
  WHILE (node<>NIL) and (s='') do
   Begin
    If node.val=r then s:=node.name;
    node:=node.next;
   End;
  if s='' then Str(r:0:0,s);
  constvalue:=s;
 End;

Function Randomm(decompile : BOOLEAN) : string;
Var s : string;
    b, bh : byte;
 Begin
  s:='';
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.. 20 66}
    Read(f,b);
    s:='random( '+IntString;
    Read(f,b);
    s:=s+', '+IntString+' )';
   End
  Else
   Begin
   End;
  Randomm:=s;
 End;

Function Get(decompile : BOOLEAN) : string;
Var s : string;
    bh : byte;
 Begin
  s:='';
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.. 16 66}
    Read(f,bh);
    s:='get( '+ConstValue(RealValue)+', ';
    Read(f,bh);
    s:=s+StringValue(decompile)+' )';
   End
  Else
   Begin
   End;
  Get:=s;
 End;

Function Tag(decompile : BOOLEAN) : string;
Var s : string;
    bh : byte;
 Begin
  s:='';
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.. 68 66}
    Read(f,bh);
    s:='tag( '+StringValue(decompile);
    Read(f,bh);
    s:=s+', '+ConstValue(RealValue)+')';
   End
  Else
   Begin
   End;
  tag:=s;
 End;

Function Vector(decompile : BOOLEAN) : string;
Var s, h : string;
    bh : byte;
 Begin
  s:='';
  if decompile then
   Begin
    s:='< ';
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.. 96 65}
    Read(f,bh); {6}

    Str(RealValue:0:3,h);
    s:=s+h+' ';

    Read(f,bh); {6}

    Str(RealValue:0:3,h);
    s:=s+h+' ';

    Read(f,bh); {6}

    Str(RealValue:0:3,h);
    s:=s+h+' >';
   End
  Else
   Begin
   End;
  vector:=s;
 End;

Function GoType(decompile : boolean) : string;
Var s : string;
    bh : byte;
 Begin
  Read(f,bh);
  Case bh of
   4 : s:=StringValue(decompile);
   7 : s:=String7Value(decompile);
   6 : s:=RealString;
   37 : s:=Randomm(decompile);
   36 : s:=Get(decompile);
   49 : s:=Tag(decompile);
   14 : s:=Vector(decompile);
  end;
  GoType:=s;
 End;

Function GoTypeS(decompile : boolean) : string;
Var s : string;
    bh : byte;
 Begin
  Read(f,bh);
  Case bh of
   4 : s:=StringValue(decompile);
   7 : s:=String7Value(decompile);
   6 : s:=RealString;
   37 : s:='$'+Randomm(decompile)+'$';
   36 : s:='$'+Get(decompile)+'$';
   49 : s:='$'+Tag(decompile)+'$';
   14 : s:=Vector(decompile);
  end;
  GoTypeS:=s;
 End;

Procedure Camera(decompile : boolean);
Var s : string;
    b, bh : byte;
    tel : integer;
    sing : single;
 Begin
  If decompile then
   Begin
    s:='camera ( ';

    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {x....}
    tel:=b;
    Read(f,b); {6}
    sing:=RealValue;
    s:=s+ConstValue(sing);
    Dec(tel,1);
    While (tel>0) and not Eof(f) do
     Begin
      s:=s+', ';
      Dec(tel,1);
      Read(f,b);
      Case b of
       4 : BEGIN s:=s+StringValue(decompile) END;
       6 : BEGIN s:=s+RealString; END;
       37 : BEGIN s:=s+'$'+Randomm(decompile)+'$'; Dec(tel,2) END;
       36 : BEGIN s:=s+'$'+Get(decompile)+'$'; Dec(tel,2) END;
       49 : BEGIN s:=s+'$'+Tag(decompile)+'$'; Dec(tel,2) END;
       14 : BEGIN s:=s+Vector(decompile); Dec(tel,3) END;
      end;
     End;
    s:=s+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  else
   Begin
   End;
 End;

Procedure Wait(decompile : boolean);
Var bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {1....}
    form1.sourcememo.lines.add(spaces+'wait ( '+GoTypeS(decompile)+' );')
   End
  Else
   Begin
   End;
 End;

Procedure Doo(decompile : boolean);
Var bh : byte;
    s : string;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {1....}
    s:=GoTypeS(decompile);

    form1.sourcememo.lines.add(spaces+'do ( '+s+' );');
   End
  Else
   Begin
   End;
 End;

Procedure Task(decompile : boolean);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    s:='task ( ';
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {1....}
    s:=s+GoTypeS(decompile);
    s:=s+' )';

    form1.sourcememo.lines.add('');
    form1.sourcememo.lines.add(spaces+s);
    form1.sourcememo.lines.add(spaces+'{');
    spaces:=spaces+Chr(9);
   End
  Else
   Begin
   End;
 End;

Procedure EndTask(decompile : boolean);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    s:='task ( ';
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.....}

    spaces:=Copy(spaces,0,Length(spaces)-1);
    form1.sourcememo.lines.add(spaces+'}');
    form1.sourcememo.lines.add('');
   End
  Else
   Begin
   End;
 End;

Procedure Sett(decompile : boolean);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    s:='set ( ';
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {2....}

    s:=s+GoTypeS(decompile)+', '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  else
   Begin
   End;
 End;

Procedure Affect(decompile : boolean);
Var bh : byte;
    s : string;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {2....}
    s:='affect ( '+GoTypeS(decompile)+', ';
    Read(f,bh); {6}
    s:=s+ConstValue(RealValue)+' )';

    form1.sourcememo.lines.add('');
    form1.sourcememo.lines.add(spaces+s);
    form1.sourcememo.lines.add(spaces+'{');
    spaces:=spaces+Chr(9);
   End
  Else
   Begin
   End;
 End;

Procedure sound(decompile : boolean);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4....}
    
    s:='sound ( '+GoTypeS(decompile)+', '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Declare(decompile : BOOLEAN);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {2....}
    Read(f,bh); {6}
    s:='declare ( '+ConstValue(RealValue)+', '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Function Eval : string;
Var b, bh : byte;
 Begin
  Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh); {x...}
  Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {4...}
  Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {....}
  Case b of
   18 : eval:='!';
   17 : eval:='=';
   16 : eval:='<';
   15 : eval:='>';
  end;
 End;

Procedure iff(decompile : BOOLEAN);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {x....}
    s:='if ( $'+GoType(decompile)+' '+eval+' '+GoType(decompile)+'$ )';

    form1.sourcememo.lines.add('');
    form1.sourcememo.lines.add(spaces+s);
    form1.sourcememo.lines.add(spaces+'{');
    spaces:=spaces+Chr(9);
   End
  Else
   Begin
   End;
 End;

Procedure Remove(decompile : BOOLEAN);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {1....}
    s:='remove ( '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Flushh(decompile : boolean);
Var b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {.....}
    form1.sourcememo.lines.add(spaces+'flush (   );');
   End
  Else
   Begin
   End;
 End;

Procedure RunScript(decompile : boolean);
Var b : byte;
    s : string;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {1....}
    s:='run ( '+GoTypeS(decompile)+' );';
    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure elsee(decompile : BOOLEAN);
Var bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {.....}

    form1.sourcememo.lines.add('');
    form1.sourcememo.lines.add(spaces+'else (  )');
    form1.sourcememo.lines.add(spaces+'{');
    spaces:=spaces+Chr(9);
   End
  Else
   Begin
   End;
 End;

Procedure Loopp(decompile : boolean);
Var s : string;
    bh : byte;
 Begin
  if decompile then
   Begin
    Read(f,bh);Read(f,bh);Read(f,bh); {...}
    Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {1....}
    s:='loop ( '+GoTypeS(decompile)+' )';

    form1.sourcememo.lines.add('');
    form1.sourcememo.lines.add(spaces+s);
    form1.sourcememo.lines.add(spaces+'{');
    spaces:=spaces+Chr(9);
   End
  Else
   Begin
   End;
 End;

Procedure WaitSignal(decompile : boolean);
Var b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {1....}

    form1.sourcememo.lines.add(spaces+'waitsignal ( '+GoTypeS(decompile)+' );');
   End
  Else
   Begin
   End;
 End;

Procedure Signal(decompile : boolean);
Var b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {1....}

    form1.sourcememo.lines.add(spaces+'signal ( '+GoTypeS(decompile)+' );');
   End
  Else
   Begin
   End;
 End;

Procedure Movee(decompile : boolean);
Var s : string;
    b, bh : byte;
    tel : integer;
    alr : boolean;
 Begin
  If decompile then
   Begin
    s:='move ( ';

    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,bh);Read(f,bh);Read(f,bh);Read(f,bh); {x....}
    tel:=b;

    alr:=false;
    While (tel>0) and not Eof(f) do
     Begin
      if alr then s:=s+', ';
      Dec(tel,1);
      Read(f,b);
      Case b of
       6 : BEGIN s:=s+RealString; END;
       37 : BEGIN s:=s+'$'+Randomm(decompile)+'$'; Dec(tel,2) END;
       36 : BEGIN s:=s+'$'+Get(decompile)+'$'; Dec(tel,2) END;
       49 : BEGIN s:=s+'$'+Tag(decompile)+'$'; Dec(tel,2) END;
       14 : BEGIN s:=s+Vector(decompile); Dec(tel,3) END;
      end;
      alr:=true;
     End;
    s:=s+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  else
   Begin
   End;
 End;

Procedure Rotate(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='rotate ( '+GoTypeS(decompile)+', '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Use(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='use ( '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Kill(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='kill ( '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Print(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='print ( '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Free(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='free ( '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure Play(decompile : boolean);
Var s : string;
    b : byte;
 Begin
  if decompile then
   Begin
    Read(f,b);Read(f,b);Read(f,b); {...}
    Read(f,b);Read(f,b);Read(f,b);Read(f,b);Read(f,b); {x....}
    s:='play ( '+GoTypeS(decompile)+', '+GoTypeS(decompile)+' );';

    form1.sourcememo.lines.add(spaces+s);
   End
  Else
   Begin
   End;
 End;

Procedure OpenCompiledfile(filename : string);
Var b : byte;
    s, h : string;
    unknown : BOOLEAN;
Begin
 form1.sourcememo.Lines.Clear;
 Assignfile(f,filename);
 Reset(f);
 form1.sourcememo.lines.add('// Generated by '+title);
 form1.sourcememo.lines.add('// Decompiled from file "'+filename+'"');
 form1.sourcememo.lines.add('');
 Read(f,b); {I}
 Read(f,b); {B}
 Read(f,b); {I}
 Read(f,b); { }
 Read(f,b); {+}
 Read(f,b); {)}
 Read(f,b); {+}
 Read(f,b); {?}
 unknown:=false;
 While not eof(f) do
  Begin
   Read(f,b);
   If b in [35,42,23,41,25,26,19,20,43,38,34,31,32,39,27,47,46,21,22,30,33,29,44,48]
     then unknown:=false;
   Case b of
    35 : Camera(true);
    42 : Doo(true);
    23 : Wait(true);
    41 : Task(true);
    25 : EndTask(true);
    26 : Sett(true);
    19 : Affect(true);
    20 : Sound(true);
    43 : Declare(true);
    38 : iff(true);
    34 : Remove(true);
    31 : Flushh(true);
    32 : Runscript(true);
    39 : elsee(true);
    27 : Loopp(true);
    47 : WaitSignal(true);
    46 : Signal(true);
    21 : Movee(true);
    22 : Rotate(true);
    30 : Use(true);
    33 : Kill(true);
    29 : Print(true);
    44 : Free(true);
    48 : Play(true);
    else BEGIN
          s:=form1.sourcememo.Lines.Strings[form1.sourcememo.lines.count-1];
          Str(b:2,h);
          If not unknown then s:=s+' *** UNKNOWN COMMAND OR ERROR: '+h
                else s:=s+' '+h;
          form1.sourcememo.Lines.Strings[form1.sourcememo.lines.count-1]:=s;
          unknown:=true;
         END;
   end;
  End;
End;

{--------------END DECOMPILER--------------------}

procedure TForm1.Open1Click(Sender: TObject);
begin
 form1.caption:=title;
 sourcememo.clear;
end;

procedure InsertMem(sing : single; st : string);
Var node : ConstListPtr;
 Begin
  if list=nil then
   Begin
    New(list);
    list.val:=sing;
    list.name:=st;
    list.next:=nil;
    list.prev:=nil;
   End
  Else
   Begin
    New(node);
    node.val:=sing;
    node.name:=st;
    node.next:=list;
    list.prev:=node;
    node.prev:=nil;
    list:=node;
   End;
 End;

procedure ClearList;
Var node : ConstListPtr;
 Begin
  while list<>nil do
   Begin
    node:=list;
    list:=list.next;
    Dispose(node);
   End;
 End;

procedure loadconstants;
Var cf : Textfile;
    s : string;
    resg : single;
    c : integer;
begin
 AssignFile(cf,Copy(ParamStr(0),0,Pos('DEVAHEB.EXE',UpperCase(ParamStr(0)))-1)+'constants.ini');
 Reset(cf);
 While not EOF(cf) do
  Begin
   Readln(cf,s);
   if s<>'' then
    Begin
     Val(Copy(s,0,Pos(',',s)-1),resg,c);
     if c=0 then
      Begin
       s:=Copy(s,Pos(',',s)+1,Length(s));
       InsertMem(resg,s);
      End;
    End;
  End;
 CloseFile(cf);
end;

procedure TForm1.FormCreate(Sender: TObject);
Var filename : string;
        tel : longint;
begin
 Xdiff:=8;
 sourcememo.width:=form1.width-xdiff;
 Ydiff:=45;
 sourcememo.height:=form1.height-ydiff;
 list:=nil;
 form1.caption:=title;
 rsptr:=Addr(rs);
 formlist:=NIL;
 LoadConstants;
 if ParamCount>0 then
  Begin
   filename:=ParamStr(1);
   If filename<>'-b' then
    Begin
     If Uppercase(Copy(filename,Length(filename)-2,3))='IBI'
       then OpenCompiledFile(filename)
       else
        Begin
         sourcememo.Lines.LoadFromFile(filename);
         form1.caption:=title+' <'+filename+'>';
        End
    End
   Else
    Begin
     For tel := 2 to ParamCount do
      Begin
       form1.caption:=form1.caption+' '+ParamStr(tel);
       filename:=ParamStr(tel);
       OpenCompiledFile(filename);
       filename:=Copy(filename,0,Length(filename)-3)+'TXT';
       Sourcememo.lines.SaveToFile(filename);
      End;
     form1.close;
     Application.Terminate;
    End;
  End;
end;

procedure TForm1.Open2Click(Sender: TObject);
begin
 opendialog1.filename:='*.ibi;*.txt';
 If Opendialog1.execute then
  Begin
   sourcememo.clear;
   If Uppercase(Copy(opendialog1.filename,Length(opendialog1.filename)-2,3))='IBI'
     then OpenCompiledFile(opendialog1.filename)
     else
      Begin
       sourcememo.Lines.LoadFromFile(opendialog1.filename);
       form1.caption:=title+' <'+opendialog1.filename+'>';
      End;
  End;
end;

procedure savenow;
 Begin
  if form1.caption=title then
  Begin
   form1.savedialog1.filename:='*.txt';
   if form1.savedialog1.execute then
    Begin
     form1.sourcememo.Lines.SaveToFile(form1.savedialog1.filename);
     form1.caption:=title+' <'+form1.savedialog1.filename+'>';
    End;
  End
 Else
  Begin
   form1.sourcememo.Lines.SaveToFile(Copy(form1.caption,Pos('<',form1.caption)+1,Pos('>',form1.caption)-1-Pos('<',form1.caption)));
  End;
 End;

procedure TForm1.Save1Click(Sender: TObject);
begin
 savenow;
end;

procedure TForm1.Saveas1Click(Sender: TObject);
begin
 savedialog1.filename:='*.txt';
 if savedialog1.execute then
  Begin
   sourcememo.Lines.SaveToFile(savedialog1.filename);
   form1.caption:=title+' <'+savedialog1.filename+'>';
  End;
end;

procedure TForm1.Exit1Click(Sender: TObject);
begin
 form1.Close;
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
 ClearList;
end;

procedure TForm1.About1Click(Sender: TObject);
begin
 form2.show;
end;

procedure TForm1.Edit1Click(Sender: TObject);
begin
 if sourcememo.canundo then undo1.enabled:=true
                       else undo1.enabled:=false;
end;

procedure TForm1.Undo1Click(Sender: TObject);
begin
 sourcememo.Undo;
end;

procedure TForm1.Cut1Click(Sender: TObject);
begin
 sourcememo.CutToClipboard;
end;

procedure TForm1.Copy1Click(Sender: TObject);
begin
 sourcememo.CopyToClipboard;
end;

procedure TForm1.Paste1Click(Sender: TObject);
begin
 sourcememo.PasteFromClipboard;
end;

procedure TForm1.SelectAll1Click(Sender: TObject);
begin
 sourcememo.SelectAll;
end;

procedure TForm1.FormResize(Sender: TObject);
begin
 sourcememo.width:=form1.width-Xdiff;
 sourcememo.height:=form1.height-Ydiff;
end;

end.
