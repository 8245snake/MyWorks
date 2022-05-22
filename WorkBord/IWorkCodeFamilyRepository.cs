﻿using WorkBord.WorkCodeFamilies;

namespace WorkBord;

/// <summary>
/// 作業コードのリポジトリ抽象
/// </summary>
public interface IWorkCodeFamilyRepository
{
    public string GetNewId();
    void Register(WorkCodeFamily workCodeFamily);
    void Delete(string familyId);
    WorkCodeFamily? FindById(string familyId);
    WorkCodeFamily[] GetAll();
}