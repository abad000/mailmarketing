using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace MailBC.UI.Infrastructure.Base
{
    public class ViewModelBase
        : INotifyPropertyChanged
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { return; };

        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyExists(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("expression must be a property expression");

            OnPropertyChanged(memberExpression.Member.Name);
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyExists(string propertyName)
        {
            PropertyInfo currentProperty = GetType().GetProperty(propertyName);
            string message = string.Format("Property Name \"{0}\" does not exist in {1}", propertyName, GetType());
            Debug.Assert(currentProperty != null, message);
        }
    }
}