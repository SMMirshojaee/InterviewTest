# InterviewTest

# شرح پروژه:
این پروژه یک سامانه درگاه پرداخت آزمایشی را شبیه سازی میکند که از 3 سرویس مجزا تشکیل شده است. سرویس payment که برای ثبت تراکنش ها و تاییدیه ها استفاده میشود. سرویس gateway که فرایند بانک را شبیه سازی میکند و سرویس notification که مامور ثبت لاگ و رسیدگی به event هاست

# معماری استفاده شده:
برای سرویس PaymentService از معماری CleanArchitecture استفاده شده که دارای 4 لایه Domain,Infrastructure,Application,Api می باشد.
سرویس GatewayService نیز از همین معماری استفاده کرده اما با توجه به اینکه نه ارتباطی با دیتابیس دارد و نه Domain خاصی دارد و صرفا ارتباط همزمان با PaymentService میگیرد، فقط لایه های Application,Api را در خود جای داده. 
سرویس NotificationService نیز با توجه به اینکه نیازی به Api ندارد، صرفا یک ConsoleApplication است که consumer های RabbitMq را در خود جای داده. البته این سرویس هم میتوانست بصورت Api و CleanArchitecture پیاده سازی شود اما باتوجه به سایز و اهمیت آن، به همین میزان از طراحی بسنده شد

# تکنولوژیهای به کار رفته
برای ساخت سرویس های Payment , Gateway از Asp.Net WebApi با نسخه Net8. استفاده شده است.NotificationService نیز یک ConsoleApp نسخه  Net8. می باشد.
برای ارتباط با دیتابیس از efCore استفاده شده.
جهت جداسازی لایه Api از Application از MediatR14.0.0 استفاده گردیده.
از RabbitMq بعنوان MessageBroker در پروژه بهره بردیم. پیاده سازی آن به کمک MassTransit8.5.7 رخ داده است.
دیتابیس استفاده شده MSSqlServer می باشد. برای ساخت جداول نیز از migration بهره مند شدیم.
برای map کردن کلاس ها و Dto ها به یکدیگر از AutoMapper16.0 استفاده شده.
پکیچ Hangfire1.8.2 برای job های تکرار شونده درPaymentService مورد استفاده قرار گرفته.
نسخه 9.0.0 از پکیج Autofac برای مدیریت DI انتخاب گردیده است.
