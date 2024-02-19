# 吾輩はメモ帳、整理はまだしない

* [ローカライズされた .NET IntelliSense ファイルのダウンロード](https://dotnet.microsoft.com/ja-jp/download/intellisense)
  * 原本となるドキュメントXMLはどこにあるのか
  * そもそもライセンスどうなってんだ
* [.NET のローカライズされた IntelliSense ファイルをインストールする方法](https://learn.microsoft.com/ja-jp/dotnet/core/install/localized-intellisense)
* .NET 側のバージョンが大きく変わった場合にディレクトリを作成して直近版をコピペ
  * メジャーバージョンでの履歴はどうでもいい

## Windows アプリケーション

* .NET Framework 4.8 で動かす
  * Windows 10 以上なら確実に動くでしょうという安易な考え
* NuGet/依存DLL は同梱しない
  * 単独 EXE 置き換えアップデートしたいので DLL がいるとしんどい
* 設定は app.config を使用する
* 一時ディレクトリ系は EXE ルートディレクトリを基点とする

