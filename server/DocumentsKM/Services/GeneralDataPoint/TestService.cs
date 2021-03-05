// using System.Collections.Generic;
// using DocumentsKM.Models;
// using DocumentsKM.Data;
// using System;
// using DocumentsKM.Dtos;
// using System.Linq;

// namespace DocumentsKM.Services
// {
//     public class TestService
//     {
//         private readonly IGeneralDataPointRepo _repository;
//         private readonly IUserRepo _userRepo;
//         private readonly IGeneralDataSectionRepo _generalDataSectionRepo;

//         public TestService(
//             IGeneralDataPointRepo generalDataPointRepo,
//             IUserRepo userRepo,
//             IGeneralDataSectionRepo generalDataSectionRepo)
//         {
//             _repository = generalDataPointRepo;
//             _userRepo = userRepo;
//             _generalDataSectionRepo = generalDataSectionRepo;
//         }

//         public string Test()
//         {

//             // 1st step
// //             SELECT DISTINCTROW Способы_аз.Наим_сп, Способы_аз.Статус_сп
// // FROM Марки INNER JOIN Способы_аз ON (Марки.материал = Способы_аз.Матер_к) AND (Марки.агрессивность = Способы_аз.Агресс_ср)
// // WHERE Марки.Id_марки=ТЕК_МАРКА


//             var envAggressivenessId = 1;
//             var constructionMaterialId = 1;

//             // Repo
//             return _context.CorrProtMethods.Where(
//                 v => v.EnvAggressiveness.Id == envAggressivenessId && v.ConstructionMaterial == constructionMaterialId).ToList();

// //             Если Статус_сп=1,2, то СпособОкр= "#Защита металлоконструкций от коррозии осуществляется " & Способы_аз.Наим_сп; 
// // 	идем к 2-му этапу
// // Если Статус_сп=3, то Ошибка = "Нет способов защиты при заданной агрессивности и материале конструкций", 
// // 	выход.
//             if (corrProtMethod.Status == 3)
//             {
//                 return "Нет способов защиты при заданной агрессивности и материале конструкций";
//             }

//             var msg = "Защита металлоконструкций от коррозии осуществляется " + corrProtMethod.Name;


//             // 2nd step
// //             SELECT DISTINCTROW Варианты_аз.Статус_аз, Степени_оч_аз.Наим_оч
// // FROM Марки INNER JOIN (Варианты_аз INNER JOIN Степени_оч_аз ON Варианты_аз.Степень_оч = Степени_оч_аз.Степень_оч) ON (Марки.материал = Варианты_аз.Матер_к) AND (Марки.агрессивность = Варианты_аз.Агресс_ср) AND (Марки.группа_газов = Варианты_аз.Группа_газов) AND (Марки.зона_эксплуат = Варианты_аз.Зона_экс)
// // WHERE Марки.Id_марки=ТЕК_МАРКА

//             var cleaningDegreeId = 1;
//             var envAggressivenessId = 1;
//             var constructionMaterialId = 1;
//             var gasGroupId = 1;
//             var operatingAreaId = 1;

//             // Repo
//             return _context.CorrProtVariants.Where(
//                 v => v.CleaningDegree.Id == cleaningDegreeId &&
//                 v.EnvAggressiveness.Id == envAggressivenessId &&
//                 v.ConstructionMaterial == constructionMaterialId &&
//                 v.GasGroup == gasGroupId &&
//                 v.OperatingArea == operatingAreaId).ToList();

//             if (corrProtVariant.Status == 2)
//             {
//                 var msg1 = "#Окраска лакокрасочными материалами не требуется.";
// 	            var msg2 = "Степень очистки поверхности стальных конструкций от окислов - " + corrProtVariant.CleaningDegree.Name + ".";
//                 return;
//             } else if (corrProtVariant.Status == 3) {
//                 return "При заданной зоне экспл., группе газов, агрессивности и материале конструкций окраска лакокрасочными материалами не возможна";
//             }

// //             // 3rd step (no needed because there aren't any null statuses)
// //             SELECT DISTINCTROW Варианты_аз.Статус_аз
// // FROM Марки INNER JOIN Варианты_аз ON (Марки.тип_ЛКМ = Варианты_аз.Тип_лп) AND (Марки.материал = Варианты_аз.Матер_к) AND (Марки.агрессивность = Варианты_аз.Агресс_ср) AND (Марки.группа_газов = Варианты_аз.Группа_газов) AND (Марки.зона_эксплуат = Варианты_аз.Зона_экс)
// // WHERE Марки.Id_марки=ТЕК_МАРКА

// //             // Repo
// //             return _context.CorrProtVariants.Where(
// //                 v => v.CleaningDegree.Id == cleaningDegreeId &&
// //                 v.EnvAggressiveness.Id == envAggressivenessId &&
// //                 v.ConstructionMaterial == constructionMaterialId &&
// //                 v.GasGroup == gasGroupId &&
// //                 v.OperatingArea == operatingAreaId).ToList();

//             // 4th step
//             SELECT DISTINCTROW Варианты_аз.Группа_лп, Варианты_аз.Кол_слоев_лп, Варианты_аз.Толщ_лпгр, Варианты_аз.Кол_слоев_гр, Покрытия_аз.Наим_лп, Грунт_аз.Наим_гр, Степени_оч_аз.Наим_оч, Покрытия_аз.Приоритет, Грунт_аз.Приоритет
// FROM Марки INNER JOIN (((Варианты_аз LEFT JOIN Покрытия_аз ON (Варианты_аз.Стойкость_лп = Покрытия_аз.Стойкость_лп) AND (Варианты_аз.Тип_лп = Покрытия_аз.Тип_лп) AND (Варианты_аз.Группа_лп = Покрытия_аз.Группа_лп)) LEFT JOIN Грунт_аз ON Покрытия_аз.Группа_гр = Грунт_аз.Группа_гр) INNER JOIN Степени_оч_аз ON Варианты_аз.Степень_оч = Степени_оч_аз.Степень_оч) ON (Марки.тип_ЛКМ = Варианты_аз.Тип_лп) AND (Марки.материал = Варианты_аз.Матер_к) AND (Марки.агрессивность = Варианты_аз.Агресс_ср) AND (Марки.группа_газов = Варианты_аз.Группа_газов) AND (Марки.зона_эксплуат = Варианты_аз.Зона_экс)
// WHERE Марки.Id_марки=ТЕК_МАРКА
// ORDER BY Покрытия_аз.Приоритет DESC , Грунт_аз.Приоритет DESC;

//             var cleaningDegreeId = 1;
//             var envAggressivenessId = 1;
//             var constructionMaterialId = 1;
//             var gasGroupId = 1;
//             var operatingAreaId = 1;



//             // Repo
//             return _context.CorrProtVariants.Where(
//                 v => v.CleaningDegree.Id == cleaningDegreeId &&
//                 v.EnvAggressiveness.Id == envAggressivenessId &&
//                 v.ConstructionMaterial == constructionMaterialId &&
//                 v.GasGroup == gasGroupId &&
//                 v.OperatingArea == operatingAreaId).ToList();

//             СпособОкр= СпособОкр & " группы " & Варианты_аз.Группа_лп & ":"
//             ОкрМонтаж= "- на монтаже - " & Покрытия_аз.Наим_лп & " в " & Варианты_аз.Кол_слоев_лп & " сл. толщиной " & Варианты_аз.Толщ_лпгр & " мкм;"
//             ОкрЗавод= "- на заводе - " & Грунт_аз.Наим_гр & " в " & Варианты_аз.Кол_слоев_гр & " сл."
//             ОчисткаПов = "Степень очистки поверхности стальных конструкций от окислов перед окраской - " & Степени_оч_аз.Наим_оч & "."

//         }
//     }
// }
