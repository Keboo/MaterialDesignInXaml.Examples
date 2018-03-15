using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;

namespace DemoApp.Model
{
    [ContentProperty("Summary")]
    public class Topic : ModelBase
    {
        public string Title { get { return _title; } set { SetValue("Title", ref _title, value); } }
        public DateTime PublishDate { get { return _publishDate; } set { SetValue("PublishDate", ref _publishDate, value); } }
        public string Summary { get { return _summary; } set { SetValue("Summary", ref _summary, value); } }
        public Uri DemoUri { get { return _demoUri; } set { SetValue("DemoUri", ref _demoUri, value); } }
        public Uri BlogUri { get { return _blogUri; } set { SetValue("BlogUri", ref _blogUri, value); } }
        public Uri CodeUri { get { return _codeUri; } set { SetValue("CodeUri", ref _codeUri, value); } }
        
        private string _title;
        private DateTime _publishDate;
        private string _summary = String.Empty;
        private Uri _demoUri;
        private Uri _blogUri;
        private Uri _codeUri;

    }
}
