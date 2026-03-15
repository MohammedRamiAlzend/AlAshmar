namespace AlAshmar.Domain.Commons;

public static class ApplicationErrors
{

    public static readonly Error InvalidCredentials = new("1", "Username or password is incorrect.", ErrorKind.Unauthorized, "اسم المستخدم أو كلمة المرور غير صحيحة.");
    public static readonly Error TokenExpired = new("3", "The authentication token has expired.", ErrorKind.Unauthorized, "انتهت صلاحية رمز المصادقة.");
    public static readonly Error InvalidToken = new("4", "The authentication token is invalid.", ErrorKind.Unauthorized, "رمز المصادقة غير صحيح.");
    public static readonly Error InsufficientPermissions = new("5", "User lacks required permissions.", ErrorKind.Forbidden, "المستخدم لا يملك الأذونات المطلوبة.");
    public static readonly Error UserNotFound = new("6", "User with the specified ID was not found.", ErrorKind.NotFound, "لم يتم العثور على المستخدم.");
    public static readonly Error UserAlreadyExists = new("7", "A user with this username already exists.", ErrorKind.Conflict, "يوجد بالفعل مستخدم بهذا الاسم.");
    public static readonly Error UserNotActive = new("8", "The user account is not active.", ErrorKind.Forbidden, "حساب المستخدم غير نشط.");

    public static readonly Error RoleNotFound = new("100", "The specified role was not found.", ErrorKind.NotFound, "لم يتم العثور على الدور المحدد.");
    public static readonly Error RoleAlreadyExists = new("101", "A role with this name already exists.", ErrorKind.Conflict, "يوجد بالفعل دور بهذا الاسم.");
    public static readonly Error CannotDeleteDefaultRole = new("102", "Cannot delete a default system role.", ErrorKind.Forbidden, "لا يمكن حذف دور النظام الافتراضي.");
    public static readonly Error RoleNotAssignedToUser = new("103", "The role is not assigned to this user.", ErrorKind.NotFound, "لم يتم تعيين الدور لهذا المستخدم.");

    public static readonly Error PermissionNotFound = new("200", "The specified permission was not found.", ErrorKind.NotFound, "لم يتم العثور على الإذن المحدد.");
    public static readonly Error PermissionAlreadyExists = new("201", "A permission with this name already exists.", ErrorKind.Conflict, "يوجد بالفعل إذن بهذا الاسم.");
    public static readonly Error PermissionNotAssignedToRole = new("202", "The permission is not assigned to this role.", ErrorKind.NotFound, "لم يتم تعيين الإذن لهذا الدور.");
    public static readonly Error SomePermissionsNotFound = new("203", "Some of the specified permissions were not found.", ErrorKind.Validation, "بعض الأذونات المحددة غير موجودة.");

    public static readonly Error TemplateNotFound = new("300", "The specified template was not found.", ErrorKind.NotFound, "لم يتم العثور على القالب المحدد.");
    public static readonly Error TemplateAlreadyExists = new("301", "A template with this name already exists.", ErrorKind.Conflict, "يوجد بالفعل قالب بهذا الاسم.");
    public static readonly Error InvalidTemplateContent = new("302", "The template content is invalid.", ErrorKind.Validation, "محتوى القالب غير صحيح.");
    public static readonly Error TemplateInUse = new("303", "The template is currently in use and cannot be deleted.", ErrorKind.Conflict, "القالب قيد الاستخدام ولا يمكن حذفه.");
    public static readonly Error UnauthorizedTemplateAccess = new("304", "You do not have access to this template.", ErrorKind.Forbidden, "ليس لديك حق الوصول إلى هذا القالب.");

    public static readonly Error RequestNotFound = new("400", "The specified request was not found.", ErrorKind.NotFound, "لم يتم العثور على الطلب المحدد.");
    public static readonly Error RequestAlreadyApproved = new("401", "The request has already been approved.", ErrorKind.Conflict, "تم بالفعل الموافقة على الطلب.");
    public static readonly Error RequestAlreadyRejected = new("402", "The request has already been rejected.", ErrorKind.Conflict, "تم بالفعل رفض الطلب.");
    public static readonly Error InvalidRequestStatus = new("403", "The request has an invalid status.", ErrorKind.Validation, "الطلب له حالة غير صحيحة.");
    public static readonly Error CannotApproveOwnRequest = new("404", "You cannot approve your own request.", ErrorKind.Forbidden, "لا يمكنك الموافقة على طلبك الخاص.");
    public static readonly Error RequesterNotFound = new("405", "The requester was not found.", ErrorKind.NotFound, "لم يتم العثور على طالب الطلب.");
    public static readonly Error ApproverNotFound = new("406", "The approver was not found.", ErrorKind.NotFound, "لم يتم العثور على الموافق.");
    public static readonly Error UnauthorizedRequestAccess = new("407", "You do not have access to this request.", ErrorKind.Forbidden, "ليس لديك حق الوصول إلى هذا الطلب.");

    public static readonly Error ResponseNotFound = new("500", "The specified response was not found.", ErrorKind.NotFound, "لم يتم العثور على الرد المحدد.");
    public static readonly Error RequestAlreadyHasResponse = new("501", "A response already exists for this request.", ErrorKind.Conflict, "يوجد بالفعل رد على هذا الطلب.");
    public static readonly Error CannotRespondToOwnRequest = new("502", "You cannot respond to your own request.", ErrorKind.Forbidden, "لا يمكنك الرد على طلبك الخاص.");
    public static readonly Error InvalidResponseContent = new("503", "The response content is invalid.", ErrorKind.Validation, "محتوى الرد غير صحيح.");

    public static readonly Error AttachmentNotFound = new("600", "The specified attachment was not found.", ErrorKind.NotFound, "لم يتم العثور على المرفق المحدد.");
    public static readonly Error InvalidAttachmentType = new("601", "The attachment type is not allowed.", ErrorKind.Validation, "نوع المرفق غير مسموح.");
    public static readonly Error AttachmentTooLarge = new("602", "The attachment file is too large.", ErrorKind.Validation, "ملف المرفق كبير جداً.");
    public static readonly Error FailedToUploadAttachment = new("603", "Failed to upload the attachment.", ErrorKind.Failure, "فشل في تحميل المرفق.");
    public static readonly Error FailedToDeleteAttachment = new("604", "Failed to delete the attachment.", ErrorKind.Failure, "فشل في حذف المرفق.");

    public static readonly Error InvalidInput = new("700", "The provided input is invalid.", ErrorKind.Validation, "الإدخال المقدم غير صحيح.");
    public static readonly Error MissingRequiredField = new("701", "A required field is missing.", ErrorKind.Validation, "حقل مطلوب مفقود.");
    public static readonly Error InvalidEmailFormat = new("702", "The email format is invalid.", ErrorKind.Validation, "صيغة البريد الإلكتروني غير صحيحة.");
    public static readonly Error InvalidPasswordLength = new("703", "The password must be at least 8 characters long.", ErrorKind.Validation, "يجب أن تكون كلمة المرور 8 أحرف على الأقل.");
    public static readonly Error PasswordsDoNotMatch = new("704", "The passwords do not match.", ErrorKind.Validation, "كلمات المرور غير متطابقة.");

    public static readonly Error InternalServerError = new("800", "An unexpected error occurred. Please try again later.", ErrorKind.Failure, "حدث خطأ غير متوقع. يرجى المحاولة لاحقاً.");
    public static readonly Error DatabaseError = new("801", "A database error occurred.", ErrorKind.Failure, "حدث خطأ في قاعدة البيانات.");
    public static readonly Error OperationFailed = new("802", "The operation could not be completed.", ErrorKind.Failure, "لم يتمكن من إكمال العملية.");
    public static readonly Error NotImplemented = new("803", "This feature has not been implemented yet.", ErrorKind.Failure, "هذه الميزة لم يتم تنفيذها بعد.");
    public static readonly Error ConcurrencyError = new("804", "The resource was modified by another user. Please refresh and try again.", ErrorKind.Conflict, "تم تعديل المورد من قبل مستخدم آخر. يرجى التحديث والمحاولة مرة أخرى.");

    public static readonly Error TransactionNotFound = new("900", "The specified transaction was not found.", ErrorKind.NotFound, "لم يتم العثور على المعاملة المحددة.");
    public static readonly Error InvalidTransactionAmount = new("901", "The transaction amount is invalid.", ErrorKind.Validation, "مبلغ المعاملة غير صحيح.");
    public static readonly Error InsufficientFunds = new("902", "Insufficient funds for this transaction.", ErrorKind.Validation, "رصيد غير كافي لهذه المعاملة.");
    public static readonly Error TransactionAlreadyProcessed = new("903", "The transaction has already been processed.", ErrorKind.Conflict, "تم معالجة المعاملة بالفعل.");
    public static readonly Error DuplicateTransaction = new("904", "A duplicate transaction was detected.", ErrorKind.Conflict, "تم كشف معاملة مكررة.");

    public static Error FileNotFound(string path, string? message = null) => new("1000", $"The specified file was not found at path : {path}.", ErrorKind.NotFound, "لم يتم العثور على الملف المحدد.");

    public static Error FileOperationFailed(string message)
        => Error.Failure("1001", $"File operation failed: {message}", "فشلت عملية الملف:");

    public static readonly Error ResourceNotFound = new("1099", "The specified resource was not found.", ErrorKind.NotFound, "لم يتم العثور على المورد المحدد.");

    public static readonly Error StudentNotFound = new("1100", "The specified student was not found.", ErrorKind.NotFound, "لم يتم العثور على الطالب المحدد.");
    public static readonly Error StudentAlreadyEnrolledInClass = new("1101", "The student is already enrolled in this class.", ErrorKind.Conflict, "الطالب مسجل بالفعل في هذا الصف.");
    public static readonly Error StudentUserNotFound = new("1102", "The student or associated user was not found.", ErrorKind.NotFound, "لم يتم العثور على الطالب أو المستخدم المرتبط به.");
    public static readonly Error NationalityNumberAlreadyExists = new("1103", "A record with this nationality number already exists.", ErrorKind.Conflict, "يوجد بالفعل سجل بهذا الرقم الوطني.");

    public static readonly Error TeacherNotFound = new("1200", "The specified teacher was not found.", ErrorKind.NotFound, "لم يتم العثور على المعلم المحدد.");
    public static readonly Error TeacherContactInfoNotFound = new("1201", "The teacher contact information was not found.", ErrorKind.NotFound, "لم يتم العثور على معلومات الاتصال بالمعلم.");
    public static readonly Error TeacherAlreadyAssignedToClass = new("1202", "The teacher is already assigned to this class.", ErrorKind.Conflict, "المعلم مسند بالفعل لهذا الصف.");
    public static readonly Error TeacherClassEnrollmentNotFound = new("1203", "The teacher class enrollment was not found.", ErrorKind.NotFound, "لم يتم العثور على تسجيل المعلم في الصف.");
    public static readonly Error TeacherUserNotFound = new("1204", "The teacher or associated user was not found.", ErrorKind.NotFound, "لم يتم العثور على المعلم أو المستخدم المرتبط به.");

    public static readonly Error ManagerNotFound = new("1300", "The specified manager was not found.", ErrorKind.NotFound, "لم يتم العثور على المدير المحدد.");
    public static readonly Error ManagerUserNotFound = new("1301", "The manager or associated user was not found.", ErrorKind.NotFound, "لم يتم العثور على المدير أو المستخدم المرتبط به.");

    public static readonly Error CourseNotFound = new("1400", "The specified course was not found.", ErrorKind.NotFound, "لم يتم العثور على المادة الدراسية المحددة.");

    public static readonly Error SemesterNotFound = new("1500", "The specified semester was not found.", ErrorKind.NotFound, "لم يتم العثور على الفصل الدراسي المحدد.");

    public static readonly Error HalaqaNotFound = new("1600", "The specified halaqa was not found.", ErrorKind.NotFound, "لم يتم العثور على الحلقة المحددة.");

    public static readonly Error FormNotFound = new("1700", "The specified form was not found.", ErrorKind.NotFound, "لم يتم العثور على النموذج المحدد.");
    public static readonly Error FormNotActive = new("1701", "The form is not active.", ErrorKind.Validation, "النموذج غير نشط.");
    public static readonly Error FormNotStarted = new("1702", "The form has not started yet.", ErrorKind.Validation, "لم يبدأ النموذج بعد.");
    public static readonly Error FormAlreadyEnded = new("1703", "The form has already ended.", ErrorKind.Validation, "انتهى النموذج بالفعل.");

    public static readonly Error EntityCannotBeNull = new("1800", "The entity cannot be null.", ErrorKind.Failure, "الكيان لا يمكن أن يكون فارغاً.");
    public static readonly Error FilterCannotBeNull = new("1801", "The filter cannot be null.", ErrorKind.Failure, "المرشح لا يمكن أن يكون فارغاً.");
    public static Error ForeignKeyConstraintViolated(string foreignKeyName)
        => new("1802", $"A foreign key constraint was violated: {foreignKeyName}", ErrorKind.Failure, $"انتهاك قيد المفتاح الأجنبي: {foreignKeyName}");

    public static readonly Error ClassNotFound = new("1900", "The specified class was not found.", ErrorKind.NotFound, "لم يتم العثور على الصف المحدد.");
}
