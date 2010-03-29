開發環境：
1. Windows 2000 Professional / Windows XP Professional
2. Visual Studio 2005 / Visual C# 2005 Express
3. .Net 2.0.50727

系統需求：
1. Windows 2000/Windows XP/Windows 2003
2. .Net Framework 2.0, http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&DisplayLang=en
3. MSXML 3.0 以上版本, http://www.microsoft.com/downloads/info.aspx?na=22&p=1&SrcDisplayLang=en&SrcCategoryId=&SrcFamilyId=&u=%2fdownloads%2fdetails.aspx%3fFamilyID%3d993c0bcf-3bcf-4009-be21-27e85e1857b1%26DisplayLang%3den
4. plink, http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html

程式特色：
1. 整合 WM 程式打包流程，從檔案收集、編碼、壓縮到寄信等，一切由程式提供一致的介面。
2. 可自動安裝/更新 Encoder Server 上的 Shell Script 程式。*
3. 支援線上更新，隨時可以取得本軟體最新版本。*
4. 支援狀態儲存，方便重複打包時的設定

程式安裝：
1. 安裝 .Net Framework 2.0,
2. 安裝 MSXML Parser 3.0 以上版本,   
3. 進入 encoder server，複製一份 arick 目錄成你要的工作目錄，例如你的工作目錄為 abc，請執行 cp -r arick abc
4. 進入你的工作目錄修改 encode_arick.sh 將 arick 的字修改成你的工作目錄名稱(例如：abc)，修改位置可以參考圖 01.gif
5. 啟動

檔案目錄結構：
AutoCollection.exe		- 主程式
/config/config.xml		- 程式組態檔
/config/project.xml		- 專案設定
/plugin/plink.exe		- 建立 ssh 連線
/plugin/ssh*			- 與 Encoder Server 溝通用的 Key
/script/arick_net3.sh		- Encoder Server 上的主程式
/script/check.php			- Encoder Server 上確認哪些檔案 Encode 失敗
/script/encode_arick.sh		- Encoder Server 上進行 Zend Encode 的主程式
/script/genmd5.php			- Encoder Server 上產生 md5 用的程式
/template/mail.template		- 信件本文範本
/template/from.template		- 寄件者清單
/template/to.template		- 收件者清單
/template/sourceList.template	- 不編碼清單
/template/readme.template	- 安裝注意事項
/images/*			- 程式使用到的圖檔
/save/*				- 預設狀態儲存路徑