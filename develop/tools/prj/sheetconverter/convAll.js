var fso = new ActiveXObject("Scripting.FileSystemObject");
var wsh = new ActiveXObject("WScript.Shell");
var curPath = fso.getParentFolderName(WScript.ScriptFullName);

// �R���o�[�g
var files = fso.GetFolder( curPath ).Files;
var e = new Enumerator( files );
for ( ; !e.atEnd(); e.moveNext() ) {
	var file = e.item();
	var ext = fso.GetExtensionName( file.Name );
	if ( ext == "xlsx" ) {
		WScript.Echo( file.Name );
		var oExe = wsh.Exec( "sheetconverter.exe -i " + curPath + "\\" + file.Name + " -p unity" );
		while( oExe.Status == 0 ) {
		}
		WScript.Echo( oExe.StdOut.ReadAll() );
	}
}

// �t�@�C�����ړ�(.bytes, .cs)
fso.CopyFile( curPath + "\\*.bytes", curPath + "\\..\\..\\Assets\\Resources\\Table" );
fso.CopyFile( curPath + "\\*.cs", curPath + "\\..\\..\\Assets\\codes\\Table" );

// �t�@�C���폜
fso.DeleteFile( curPath + "\\*.bytes" )
fso.DeleteFile( curPath + "\\*.cs" )