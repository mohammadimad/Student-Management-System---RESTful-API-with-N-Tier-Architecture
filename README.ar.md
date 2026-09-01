[English](README.md) | **العربية**

# واجهة برمجة تطبيقات نظام إدارة الطلاب

واجهة RESTful API تعليمية متكاملة مبنية باستخدام ASP.NET Core ومعمارية من ثلاث طبقات. يدير المشروع سجلات الطلاب من خلال طبقة HTTP واضحة، وخدمات لمنطق الأعمال، ومستودع ADO.NET غير متزامن يعتمد على إجراءات SQL Server المخزنة.

## حالة المشروع

الواجهة مطبقة وتُبنى بنجاح باستخدام .NET 8 مع **0 تحذيرات و0 أخطاء**. تتضمن عمليات CRUD، والتحقق من المدخلات، وتوثيق Swagger، والإعداد الآمن، وسكربت إعداد قاعدة البيانات، ورموز حالة HTTP مناسبة، وأمثلة جاهزة للطلبات.

## المزايا

- إنشاء سجلات الطلاب وقراءتها وتعديلها وحذفها.
- إرجاع الطلاب الناجحين (`Grade >= 50`).
- حساب متوسط العلامات.
- التحقق تلقائيًا من الأسماء والأعمار والعلامات.
- إرجاع رموز HTTP معيارية، ومنها `201` و`204` و`400` و`404`.
- الوصول غير المتزامن إلى SQL Server من خلال إجراءات مخزنة ومعاملات آمنة.
- فصل مسؤوليات الواجهة ومنطق الأعمال والوصول إلى البيانات.
- إعداد الاعتماديات باستخدام Dependency Injection المدمج.
- إرجاع Problem Details معيارية للأخطاء غير المعالجة.
- استعراض المسارات واختبارها من خلال Swagger UI.
- فحص جاهزية التطبيق من خلال `/health`.

## التقنيات المستخدمة

- .NET 8
- ASP.NET Core Web API
- C#
- SQL Server
- ADO.NET باستخدام `Microsoft.Data.SqlClient`
- Swagger / OpenAPI

## المعمارية

```text
StudentApi.sln
|-- MyFirstRestAPI_Porject/          # طبقة العرض وواجهة API
|   |-- Contracts/                   # عقود الطلب والاستجابة مع قواعد التحقق
|   |-- Controllers/                 # مسارات REST
|   |-- Properties/launchSettings.json
|   `-- Program.cs                   # تشغيل التطبيق وإعداد DI
|-- StudentAPIBusinessLayer/         # طبقة منطق الأعمال
|   |-- IStudentService.cs
|   |-- StudentDto.cs
|   `-- StudentService.cs
|-- StudentDataAccessLayer/          # طبقة الوصول إلى البيانات
|   |-- IStudentRepository.cs
|   |-- SqlStudentRepository.cs
|   `-- StudentRecord.cs
`-- database/
    `-- StudentManagement.sql        # قاعدة البيانات والجدول والإجراءات المخزنة
```

مسار الاعتماديات:

```text
HTTP Request -> StudentsController -> IStudentService -> IStudentRepository -> SQL Server
```

تحافظ الواجهات البرمجية على ضعف الترابط بين الطبقات، بينما يتولى ASP.NET Core إنشاء التنفيذات المطلوبة من خلال Dependency Injection.

## مسارات API

| الطريقة | المسار | الوصف | الاستجابة الناجحة |
|---|---|---|---|
| `GET` | `/health` | فحص جاهزية الواجهة | `200 OK` |
| `GET` | `/api/students` | إرجاع جميع الطلاب | `200 OK` |
| `GET` | `/api/students/{id}` | إرجاع طالب واحد | `200 OK` |
| `GET` | `/api/students/passed` | إرجاع الطلاب بعلامة 50 أو أكثر | `200 OK` |
| `GET` | `/api/students/average-grade` | إرجاع متوسط علامات الطلاب | `200 OK` |
| `POST` | `/api/students` | إنشاء طالب | `201 Created` |
| `PUT` | `/api/students/{id}` | تعديل طالب | `200 OK` |
| `DELETE` | `/api/students/{id}` | حذف طالب | `204 No Content` |

تعيد السجلات غير الموجودة `404 Not Found`، بينما تعيد أجسام الطلب غير الصحيحة `400 Bad Request` مع تفاصيل التحقق.

### جسم الطلب

```json
{
  "name": "Mohammad Abdelfattah",
  "age": 23,
  "grade": 95
}
```

قواعد التحقق:

- `name`: مطلوب، ويتراوح طوله بين 2 و100 حرف.
- `age`: بين 1 و150.
- `grade`: بين 0 و100.

## التشغيل محليًا

### المتطلبات

- .NET 8 SDK
- SQL Server أو SQL Server Express
- SQL Server Management Studio أو أداة `sqlcmd`

### 1. استنساخ المستودع

```bash
git clone https://github.com/mohammadimad/Student-Management-System---RESTful-API-with-N-Tier-Architecture.git
cd Student-Management-System---RESTful-API-with-N-Tier-Architecture
```

### 2. إنشاء قاعدة البيانات

افتح `database/StudentManagement.sql` في SQL Server Management Studio ونفّذه، أو استخدم:

```powershell
sqlcmd -S . -E -i database/StudentManagement.sql
```

ينشئ السكربت:

- قاعدة البيانات `StudentManagementDb`.
- جدول `Students` مع قيود صحة البيانات.
- جميع الإجراءات المخزنة التي يحتاجها المستودع.

### 3. إعداد الاتصال

تستخدم بيئة التطوير Windows Authentication افتراضيًا:

```text
Server=.;Database=StudentManagementDb;Trusted_Connection=True;TrustServerCertificate=True;
```

عدّل `appsettings.Development.json` بما يتناسب مع نسخة SQL Server المحلية. عند استخدام بيانات دخول أو بيئات نشر، استخدم .NET User Secrets أو متغيرات البيئة بدل حفظها في Git:

```powershell
dotnet user-secrets set "ConnectionStrings:StudentDatabase" "YOUR_CONNECTION_STRING" --project MyFirstRestAPI_Porject/StudentApi.csproj
```

أو استخدم متغير البيئة:

```text
ConnectionStrings__StudentDatabase=YOUR_CONNECTION_STRING
```

### 4. استعادة الحزم وبناء المشروع

```bash
dotnet restore StudentApi.sln
dotnet build StudentApi.sln --no-restore
```

### 5. تشغيل الواجهة

```bash
dotnet run --project MyFirstRestAPI_Porject/StudentApi.csproj
```

افتح [http://localhost:5215/swagger](http://localhost:5215/swagger) لاستعراض الواجهة. توجد أمثلة جاهزة أيضًا في `MyFirstRestAPI_Porject/StudentAPi.http`.

## عمليات قاعدة البيانات

تستخدم طبقة البيانات الإجراءات المخزنة التالية:

| الإجراء المخزن | الغرض |
|---|---|
| `SP_GetAllStudents` | إرجاع جميع الطلاب |
| `SP_GetPassedStudents` | إرجاع الطلاب بعلامة 50 أو أكثر |
| `SP_GetAverageGrade` | حساب متوسط العلامات |
| `SP_GetStudentById` | إرجاع طالب حسب المعرّف |
| `SP_AddStudent` | إضافة طالب وإرجاع المعرّف الجديد |
| `SP_UpdateStudent` | تعديل طالب وإرجاع عدد الصفوف المتأثرة |
| `SP_DeleteStudent` | حذف طالب وإرجاع عدد الصفوف المتأثرة |

## التحقق من البناء

تم التحقق باستخدام:

```bash
dotnet restore StudentApi.sln
dotnet build StudentApi.sln --configuration Release --no-restore
dotnet list StudentApi.sln package --vulnerable --include-transitive
```

النتيجة الحالية:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
No vulnerable packages found from the configured NuGet sources.
```

تم كذلك اختبار تشغيل التطبيق ومسار الصحة والتحقق التلقائي من الطلبات ومستند Swagger وجميع المسارات المسجلة. تحتاج العمليات المرتبطة بالبيانات إلى SQL Server يعمل ومهيأ باستخدام السكربت المرفق.

## ملاحظات أمنية

- لا توجد أسماء مستخدمين أو كلمات مرور لقواعد البيانات داخل الشيفرة المصدرية.
- احتفظ بسلاسل الاتصال الإنتاجية في متغيرات البيئة أو مدير أسرار.
- يستخدم اتصال التطوير المرفق Windows Authentication وهو مخصص للاستخدام المحلي فقط.
- أضف المصادقة والتفويض المعتمد على الأدوار قبل إتاحة العمليات الإدارية للعامة.
- استخدم HTTPS وتسجيلًا ومراقبة مناسبين عند النشر.

## المطوّر

[محمد عماد عبد الفتاح](https://github.com/mohammadimad)
