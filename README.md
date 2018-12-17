# ReplaceOnAssetsUpdate

## 概要

アセット更新時に自動でテキスト置換を行うエディタ拡張です。

## インストール

[このリポジトリのzipファイル](https://github.com/Taremin/ReplaceOnAssetsUpdate/archive/master.zip)をダウンロードして、解凍したものをアセットの `Plugins` フォルダにコピーします。

## 使い方

1. 置換条件を書いた JSON ファイルを作成する
2. Unityのメニューから `Window` -> `ReplaceOnAssetsUpdate` をクリック
3. `Select JSON` で作成した JSON ファイルを指定

これでアセットの更新時に自動でテキスト置換ができるようになります。

## JSON 書式例

```json
{
	"replaceSettings": [
		{
			"sourcePathPattern": "置換対象とするパスの正規表現",
			"sourcePathReplace": "置換結果を出力するパス",
			"replace": [
				{
					"textPattern": "置換に使う正規表現",
					"textReplace": "置換するテキスト"
				}
			]
		}
	]
}
```

配列になってるところは複数書くことが出来ます。

## ライセンス

[MIT](./LICENSE)
