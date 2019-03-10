package main

import (
	"flag"
	"fmt"
	"os"
	"path/filepath"
	"strconv"
	"strings"
	"unicode"

	"github.com/tealeg/xlsx"
)

func main() {
	// 引数
	var (
		xlsxFileName      = flag.String("i", "", "Input Excel File Name(.xlsx)")
		outputTSVFileName = flag.String("o", "", "Output Tab Separable Values(TSV) File Path (no extention).")
		platform          = flag.String("p", "unity", "Platform Name(unity, cpp, cs)")
	)

	flag.Parse()

	if *xlsxFileName == "" {
		fmt.Printf("No input excel file.")
		os.Exit(-1)
	}
	if *outputTSVFileName == "" {
		_, fileName := filepath.Split(*xlsxFileName)
		*outputTSVFileName = strings.Split(fileName, ".")[0]
		fmt.Println("No set outputTSVFileName fileName: " + fileName)
	} else {
		*outputTSVFileName = strings.Split(*outputTSVFileName, ".")[0]
	}

	excel, err := xlsx.OpenFile(*xlsxFileName)
	if err != nil {
		fmt.Printf(err.Error())
		os.Exit(-1)
	}

	// すべてのシートをイテレート
	// B1がTRUE、B2にRow、B3にCol値が入っているシートのみ有効
	sheetNum := len(excel.Sheets)
	for i := 0; i < sheetNum; i++ {
		sheet := excel.Sheets[i]
		validate := sheet.Rows[0].Cells[1].Value
		if validate != "1" {
			continue
		}
		row, err1 := strconv.Atoi(sheet.Rows[1].Cells[1].Value)
		col, err2 := strconv.Atoi(sheet.Rows[2].Cells[1].Value)
		if err1 != nil || err2 != nil {
			continue
		}

		// パラメータと型項目を格納
		var parameterNames []string
		var parameterValidate []bool
		var typeNames []string
		cellNum := len(sheet.Rows[row].Cells) - col
		for i := 0; i < cellNum; i++ {
			name := sheet.Rows[row].Cells[col+i].Value
			str := []rune(name)
			if len(str) == 0 {
				break
			}
			if str[0] == '#' {
				// コメント列
				parameterValidate = append(parameterValidate, false)
				continue
			}
			typeName := sheet.Rows[row+1].Cells[col+i].Value
			if len(typeName) == 0 || checkTypeName(typeName) == false {
				break
			}
			parameterValidate = append(parameterValidate, true)
			parameterNames = append(parameterNames, name)
			typeNames = append(typeNames, typeName)
		}
		if len(parameterNames) == 0 {
			fmt.Printf("No parameter in sheet '" + sheet.Name + "'.")
			os.Exit(-1)
		}

		// データ格納
		sepchar := "\t"
		recordNum := len(parameterNames)
		strs := make([]string, recordNum)
		dataRowNum := len(sheet.Rows) - row - 2
		var records []string
		records = append(records, strconv.Itoa(recordNum))  // パラメータ数
		records = append(records, strconv.Itoa(dataRowNum)) // データ行数
		records = append(records, strings.Join(parameterNames, sepchar))
		records = append(records, strings.Join(typeNames, sepchar))
		for r := row + 2; r < len(sheet.Rows); r++ {
			idx := 0
			for c := 0; c < cellNum; c++ {
				if parameterValidate[c] == true {
					strs[idx] = sheet.Rows[r].Cells[col+c].Value
					idx++
				}
			}
			records = append(records, strings.Join(strs, sepchar))
		}

		allData := strings.Join(records, "\n")

		// ファイル出力
		ext := ""
		switch *platform {
		case "unity":
			ext = "bytes"
			break
		case "cpp":
			ext = "tsv"
			break
		case "cs":
			ext = "tsv"
			break
		}
		outputFileName := *outputTSVFileName + "_" + sheet.Name + "." + ext
		file, err := os.Create(outputFileName)
		if err != nil {
			os.Exit(-1)
		}
		file.WriteString(allData)
		file.Close()

		fmt.Printf(*outputTSVFileName + ": " + sheet.Name + ": success -> " + outputFileName + "\n")

		// 指定プラットフォームのテーブルアクセスコードを生成
		// ファイル名の文頭を大文字にする
		accFileBaseName := *outputTSVFileName + "_" + sheet.Name
		accFileBaseNameUTF8 := []rune(accFileBaseName)
		accFileBaseNameUTF8[0] = unicode.ToUpper(accFileBaseNameUTF8[0])
		accFileBaseName = string(accFileBaseNameUTF8[:])
		switch *platform {
		case "unity":
			createTableAccCodeForUnity(parameterNames, typeNames, outputFileName, accFileBaseName)
		case "cpp":
			createTableAccCodeForCpp(parameterNames, typeNames, outputFileName, accFileBaseName)
		case "cs":
			createTableAccCodeForCS(parameterNames, typeNames, outputFileName, accFileBaseName)
		}
	}

}

// 型名をチェック
func checkTypeName(typeName string) bool {
	if typeName == "string" {
		return true
	} else if typeName == "int" {
		return true
	} else if typeName == "float" {
		return true
	}
	return false
}

// Unity用のアクセッサクラスコードを生成
func createTableAccCodeForUnity(parameterNames []string, typeNames []string, outputFileName string, baseName string) {

	className := baseName
	tableBaseName := strings.Split(outputFileName, ".")[0]

	typePrefix := func(typeStr string) string {
		switch typeStr {
		case "int":
			return "i"
		case "float":
			return "f"
		case "string":
			return "s"
		}
		return ""
	}
	paramStores := ""
	for i := 0; i < len(parameterNames); i++ {
		paramStores += "\t\tparam." + parameterNames[i] + "_ = values[ \"" + parameterNames[i] + "\" ]." + typePrefix(typeNames[i]) + "Val_;\n"
	}
	paramStores += "\t\tparams_[ values[ \"" + parameterNames[0] + "\" ]." + typePrefix(typeNames[0]) + "Val_ ] = param;\n"

	paramList := ""
	for i := 0; i < len(parameterNames); i++ {
		paramList += "\t\tpublic " + typeNames[i] + " " + parameterNames[i] + "_;\n"
	}

	str := `using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ` + className + ` : Table {
	public static ` + className + ` getInstance() {
		return instance_;
	}
	` + className + `() {
		create( "Table/` + tableBaseName + `" );
	}

	// 1レコードを格納
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Param();
` + paramStores + `
		paramList_.Add( param );
	}

	// データ数を取得
	public int getRowNum() {
		return params_.Count;
	}

	// パラメータを取得
	public Param getParam( string id ) {
		return params_[ id ];
	}

	// パラメータをインデックスで取得
	public Param getParamFromIndex( int idx ) {
		if ( idx >= paramList_.Count )
			return null;
		return paramList_[ idx ];
	}
	
	public class Param {
` + paramList + `
	}
	static ` + className + ` instance_ = new ` + className + `();
	Dictionary< string, Param > params_ = new Dictionary<string, Param>();
	List<Param> paramList_ = new List<Param>();
}`

	file, err := os.Create(className + ".cs")
	if err != nil {
		os.Exit(-1)
	}
	file.WriteString(str)
	file.Close()
}

// C++用のアクセッサクラスコードを生成
func createTableAccCodeForCpp(parameterNames []string, typeNames []string, outputFileName string, baseName string) {

}

// C#用のアクセッサクラスコードを生成
func createTableAccCodeForCS(parameterNames []string, typeNames []string, outputFileName string, baseName string) {

}
