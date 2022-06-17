using MyWorkDashboard.Shared.ToDoTasks;

namespace MyWorkDashboard.Shared;

public interface IToDoRepository
{

    /// <summary>
    /// 新規IDを払い出す
    /// </summary>
    /// <returns>業務ID</returns>
    public Task<string> GetNewIdAsync();

    /// <summary>
    /// ToDoを登録する。Idが同じものがあれば更新される。
    /// </summary>
    /// <param name="item">登録するToDo項目</param>
    Task RegisterAsync(ToDoItem item);

    /// <summary>
    /// todoを削除する
    /// </summary>
    /// <param name="id">業務ID</param>
    Task DeleteAsync(string id);

    /// <summary>
    /// IDでtodoを検索する
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>todo項目</returns>
    Task<ToDoItem?> FindByIdAsync(string id);

    /// <summary>
    /// 日付でtodoを検索する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>todoの配列</returns>
    Task<ToDoItem[]> FindByDateAsync(DateOnly date);

    /// <summary>
    /// 指定日より古いToDoを取得する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>todoの配列</returns>
    Task<ToDoItem[]> FindItemsBeforeThanAsync(DateOnly date);
}