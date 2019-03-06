package main

import (
	"flag"
	"fmt"
	"os"
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

	//	*xlsxFileName = "asterism.xlsx"

	if *xlsxFileName == "" {
		fmt.Printf("No input excel file.")
		os.Exit(-1)
	}
	if *outputTSVFileName == "" {
		*outputTSVFileName = strings.Split(*xlsxFileName, ".")[0]
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
		outputFileName := *outputTSVFileName + "_" + sheet.Name + ".tsv"
		file, err := os.Create(outputFileName)
		if err != nil {
			os.Exit(-1)
		}
		file.WriteString(allData)
		file.Close()

		fmt.Printf(*outputTSVFileName + ": " + sheet.Name + ": success -> " + outputFileName + "\n")

		// 指定プラットフォームのテーブルアクセスコードを生成
		// ファイル名の文頭を大文字にする
		accFileBaseName := *outputTSVFileName + "_" + sheet.Name + "_acc"
		accFileBaseNameUTF8 := []rune(accFileBaseName)
		accFileBaseNameUTF8[0] = unicode.ToUpper(accFileBaseNameUTF8[0])
		accFileBaseName = string(accFileBaseNameUTF8[:])
		switch *platform {
		case "unity":
			createTableAccCodeForUnity(outputFileName, accFileBaseName)
		case "cpp":
			createTableAccCodeForCpp(outputFileName, accFileBaseName)
		case "cs":
			createTableAccCodeForCS(outputFileName, accFileBaseName)
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
func createTableAccCodeForUnity(outputFileName string, baseName string) {

}

// C++用のアクセッサクラスコードを生成
func createTableAccCodeForCpp(outputFileName string, baseName string) {

}

// C#用のアクセッサクラスコードを生成
func createTableAccCodeForCS(outputFileName string, baseName string) {

}
