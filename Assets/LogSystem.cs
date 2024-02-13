
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
	//　表示するログの種類の列挙型
	public enum LogType {
		All,
		Time,
		Event
	}

	//　ログ出力先テキスト
	[SerializeField]
	private Text logText;
	//　全データ
	private List<string> allLogs;
	//　時間のデータ
	private List<string> timerLogs;
	//　イベントのデータ
	private List<string> eventLogs;
	//　現在表示するログの種類
	[SerializeField]
	private LogType logTypeToDisplay = LogType.All;
	//　ログを保存する数
	[SerializeField]
	private int allLogDataNum = 10;
	[SerializeField]
	private int timerLogDataNum = 10;
	[SerializeField]
	private int eventLogDataNum = 10;
	//　縦のスクロールバー
	[SerializeField]
	private Scrollbar verticalScrollbar;
	private StringBuilder logTextStringBuilder;

    // Start is called before the first frame update
    void Start()
    {
		allLogs = new List<string>();
		timerLogs = new List<string>();
		eventLogs = new List<string>();
		logTextStringBuilder = new StringBuilder();
    }
	//　ログテキストの追加
	public void AddLogText(string logText, LogType logType) {
		//　ログテキストの追加
		allLogs.Add(logText);
		if(logType == LogType.Event) {
			eventLogs.Add(logText);
		} else if(logType == LogType.Time) {
			timerLogs.Add(logText);
		}
		//　ログの最大保存数を超えたら古いログを削除
		if(allLogs.Count > allLogDataNum) {
			allLogs.RemoveRange(0, allLogs.Count - allLogDataNum);
		}
		if(timerLogs.Count > timerLogDataNum) {
			timerLogs.RemoveRange(0, timerLogs.Count - timerLogDataNum);
		}
		if (eventLogs.Count > eventLogDataNum) {
			eventLogs.RemoveRange(0, eventLogs.Count - eventLogDataNum);
		}
		//　ログテキストの表示
		if (logTypeToDisplay == LogType.All || logTypeToDisplay == logType) {
			ViewLogText();
		}
	}

	//　上からログを追加するバージョン
	//　ログテキストの表示
	public void ViewLogText() {
		logTextStringBuilder.Clear();
		List<string> selectedLogs = new List<string>();
		//　ログタイプによって表示するログを変える
		if (logTypeToDisplay == LogType.All) {
			selectedLogs = allLogs;
		} else if (logTypeToDisplay == LogType.Event) {
			selectedLogs = eventLogs;
		} else if (logTypeToDisplay == LogType.Time) {
			selectedLogs = timerLogs;
		}
		
		foreach (var log in selectedLogs) {
			logTextStringBuilder.Insert(0, log + Environment.NewLine);
		}
		logText.text = logTextStringBuilder.ToString().TrimEnd();
		UpdateScrollBar();
	}
	// スクロールバーの位置の更新
	public void UpdateScrollBar() {
		verticalScrollbar.value = 1f;
	}
}
