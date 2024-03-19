using System.Linq.Expressions;

namespace CloudyWing.OrderingSystem.Infrastructure.Util {
    public static class ExceptionUtils {
        public static void ThrowIfNull<T>(Expression<Func<T?>> expression) where T : class {
            T argument = expression.Compile().Invoke() ?? throw new ArgumentNullException(GetMemberName(expression));
        }

        public static void ThrowIfNull<T>(Expression<Func<T?>> expression) where T : struct {
            if (!expression.Compile().Invoke().HasValue) {
                throw new ArgumentNullException(GetMemberName(expression));
            }
        }

        public static void ThrowIfNullOrWhiteSpace(Expression<Func<string?>> expression) {
            string? value = expression.Compile().Invoke();
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException("不得為 Null 或空白字元。", GetMemberName(expression));
            }
        }

        public static void ThrowItemNotFound() {
            throw new InvalidOperationException("資料項目不存在。");
        }

        public static void ThrowIfItemNotFound<T>(T? obj) where T : class {
            if (obj is null) {
                throw new InvalidOperationException("資料項目不存在。");
            }
        }

        private static string GetMemberName<T>(Expression<Func<T>> expression) {
            if (expression.Body is not MemberExpression expressionBody) {
                throw new ArgumentException(null, nameof(expression));
            }
            return expressionBody.Member.Name;
        }
    }
}
