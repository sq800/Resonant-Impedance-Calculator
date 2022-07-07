using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 谐振阻抗计算器
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double f1, lc1, l1, c1, f2, l2, xl, f3, c3, xc;

        

        public MainWindow()
        {
            InitializeComponent();
        }

        private double StrToDou(string str)
        {
            double dou;
            //若存在M
            if (str.Contains('M'))
            {
                str = str.Split('M')[0];
                dou = Convert.ToDouble(str);
                dou = dou * 1e6;
                return dou;
            }
            else if (str.Contains('k'))
            {
                str = str.Split('k')[0];
                dou = Convert.ToDouble(str);
                dou = dou * 1e3;
                return dou;
            }
            else if (str.Contains('m'))
            {
                str = str.Split('m')[0];
                dou = Convert.ToDouble(str);
                dou = dou * 1e-3;
                return dou;

            }
            else if (str.Contains('u'))
            {
                str = str.Split('u')[0];
                dou = Convert.ToDouble(str);
                dou = dou * 1e-6;
                return dou;

            }
            else if (str.Contains('n'))
            {
                str = str.Split('n')[0];
                dou = Convert.ToDouble(str);
                dou = dou * 1e-9;
                return dou;

            }
            else if (str.Contains('p'))
            {
                str = str.Split('p')[0];
                dou = Convert.ToDouble(str);
                dou *= 1e-12;
                return dou;
            }
            else
            {
                try
                {
                    dou = Convert.ToDouble(str);
                    return dou;
                }
                catch
                {
                    MessageBox.Show("非法字符");
                    return 0;
                }
            }
        }

        private string DouToStr(double dou)
        {
            if (dou == 0)
            {
                return "0";
            }
            else if (dou >= 1)
            {
                string str = dou.ToString();
                if (str.Contains('.'))
                {
                    string a = str.Split('.')[0];
                    string b = str.Split('.')[1];
                    b = b.Substring(0,3);
                    a = a + '.' + b;
                    return a;
                }
                return str;
            }
            else if (dou >= 1e-4)
            {
                string str = dou.ToString();
                int index = str.IndexOf('.');
                // subStr为小数点后的数字
                string subStr = str.Substring(index + 1, str.Length - 1 - index);
                char[] chars = subStr.ToCharArray();
                //MessageBox.Show(new String(chars));
                int start = 0;
                char[] result = new char[10];
                //MessageBox.Show(new String(chars));
                // 循环查询小数点后的0，跳到有值的位置，如0.004，跳到4
                for (int i = 1; i < chars.Length; i++)
                {
                    if (chars[start] != '0')
                    {
                        break;
                    }
                    else
                    {
                        if (chars[start] != chars[i])
                        {
                            start = i;
                            break;
                        }
                    }
                }
                result[0] = chars[start];
                result[1] = '.';
                result[2] = chars[start + 1];
                result[3] = chars[start + 2];
                result[4] = chars[start + 3];
                result[5] = 'E';
                result[6] = '-';
                string n = Convert.ToString(start + 1);

                string resultStr = (new string(result)) + n;
                return resultStr;
            }
            else  // 小于0.0001会自动转为科学计数法，需要特殊处理
            {
                string str = dou.ToString();
                string start = str.Substring(0, 5);
                int e = str.LastIndexOf('E');
                //MessageBox.Show(e.ToString());
                string end = str.Substring(e);
                string resultStr = start + end;
                return resultStr;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = e.Source as TextBox;
            box.Background = Brushes.Pink;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            TextBox box = e.Source as TextBox;
            //double result, dou;
            if (box.Text != null)
            {
                if (box.Text.Length == 0)
                {
                    LC.Text = box.Text;

                }
                else
                {
                    f1 = StrToDou(box.Text);
                    //F1 = dou;
                    lc1 = Math.Sqrt(1 / (2 * f1 * Math.PI));
                    LC.Text = DouToStr(lc1);
                }
            }
        }

        private void L_KeyDown(object sender, KeyEventArgs e)
        {
            if (LC.Text.Length != 0&&L.Text!="")
            {
                //取得L1的值
                l1 = StrToDou(L.Text);
                double result = lc1 / l1;
                //保存C1的值
                c1 = result;
                //C1的值渲染到页面上
                C.Text = DouToStr(result);
            }
            else
            {
                C.Text = "";
            }
        }
        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            if (LC.Text.Length != 0&&C.Text!="")
            {
                //double lcValue = LC1;
                //取得C1的值
                c1 = StrToDou(C.Text);
                //计算L1的值
                l1 = lc1 / c1;
                //把L的值渲染到页面上
                L.Text = DouToStr(l1);
            }
            else
            {
                L.Text = "";
            }
        }

        

        // F2的事件
        private void TextBox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            if (F2.Text.Length != 0 && L2.Text.Length != 0)
            {
                //取得f2的值
                f2 = StrToDou(F2.Text);
                //取得L2的值
                l2 = StrToDou(L2.Text);
                //计算xl的值
                xl = 2 * Math.PI * f2 * l2;
                //渲染xl的值
                XL.Text = DouToStr(xl);
            }
            else
            {
                xl = 0;
                XL.Text = "";
            }
        }

        //F3事件
        private void F3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (F3.Text.Length != 0 && C3.Text.Length != 0)
            {
                //取得f3的值
                f3 = StrToDou(F3.Text);
                //取得C3的值
                c3 = StrToDou(C3.Text);
                //计算XC的值
                xc = 1 / (2 * Math.PI * f3 * c3);
                //渲染XC的值
                XC.Text = DouToStr(xc);
            }
            else
            {
                xc = 0;
                XC.Text = "";
            }
        }
    }
}
