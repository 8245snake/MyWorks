using MyWorkDashboard.Shared.Duties;

namespace MyWorkDashboard.Shared;

/// <summary>
/// 業務データのリポジトリ抽象
/// </summary>
public interface IDutyRepository
{

    /// <summary>
    /// 新規IDを払い出す
    /// </summary>
    /// <returns>業務ID</returns>
    Task<string> GetNewIdAsync();

    /// <summary>
    /// 業務を登録する。Idが同じものがあれば更新される。
    /// </summary>
    /// <param name="duty">業務</param>
    Task RegisterAsync(Duty duty);

    /// <summary>
    /// 業務を削除する
    /// </summary>
    /// <param name="dutyId">業務ID</param>
    Task DeleteAsync(string dutyId);

    /// <summary>
    /// IDで業務を検索する
    /// </summary>
    /// <param name="dutyId">業務ID</param>
    /// <returns>業務</returns>
    Task<Duty?> FindByIdAsync(string dutyId);

    /// <summary>
    /// 日付で業務を検索する
    /// </summary>
    /// <param name="date">日付</param>
    /// <returns>業務の配列</returns>
    Task<Duty[]> FindByDateAsync(DateOnly date);
}