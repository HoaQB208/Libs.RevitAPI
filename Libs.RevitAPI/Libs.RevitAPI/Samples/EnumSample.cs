namespace Libs.RevitAPI.Samples
{
    internal class EnumSample
    {
        /// Tham khảo thêm trong EnumUtils

        //MyEnum
        //enum MyEnum
        //{
        //    [Description("Name 1")]
        //    Name1 = 1,
        //    [Description("Name 2")]
        //    Name2 = 2,
        //}

        // Lấy danh sách tên các enum của EnumType
        ///  string[] enumNames = Enum.GetNames(typeof(MyEnum));

        // Lấy danh sách enum của EnumType
        ///  Array enums = Enum.GetValues(typeof(MyEnum));


        // ForWPF
        /// VM: public ObservableCollection<MyEnum> EnumValues { get; } = new ObservableCollection<MyEnum>(Enum.GetValues(typeof(MyEnum)).Cast<MyEnum>());
        /// 
        /// View: <ComboBox ItemsSource="{Binding EnumValues}" SelectedItem="{Binding SelectedEnumValue}" />

    }
}
