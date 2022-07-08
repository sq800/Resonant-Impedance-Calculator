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
        /*******************************************************************************
        ** @name      函数名  : HandleNumber
        ** @brief     功能    : 为大于等于1的数保留不超过3位小数位
        ** @param     参数    : double	无
        ** @return    返回    : string	无
        ** @author    作者    : ysq
        ** @date      创建日期: 2022/7/8
        ** ----------------------------------------------------------------------------
        ** @change by 修改者  :
        ** @date      修改日期:
        ** @brier     修改内容:
        *******************************************************************************/
        private string HandleNumber(double dou)
        {
            string str = dou.ToString();
            if (str.Contains('.'))
            {
                string a = str.Split('.')[0];
                string b = str.Split('.')[1];
                //若超过3位则只保留3位
                if (b.Length >= 3)
                {
                    b = b.Substring(0, 3);

                }
                //若不超过3位则直接拼接
                a = a + '.' + b;
                return a;
            }
            return str;
        }

        /*******************************************************************************
        ** @name      函数名  : HandleDecimal
        ** @brief     功能    : 小数的处理方法
        ** @param     参数    : double dou	
        ** @return    返回    : string	
        ** @author    作者    : ysq
        ** @date      创建日期: 2022/7/8
        ** ----------------------------------------------------------------------------
        ** @change by 修改者  :
        ** @date      修改日期:
        ** @brier     修改内容:
        *******************************************************************************/
        private string HandleDecimal(double dou)
        {
            string str = dou.ToString();
            int index = str.IndexOf('.');
            // subStr为小数点后的数字
            string subStr = str.Substring(index + 1, str.Length - 1 - index);
            char[] chars = subStr.ToCharArray();
            //MessageBox.Show(new String(chars));
            int start = 0;
            char[] result = new char[20];
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
            string resultStr = (new string(result)) + '0' + n;
            return resultStr;
        }

        /*******************************************************************************
        ** @name      函数名  : HandleScientific
        ** @brief     功能    : 科学计数法只保留三位小数
        ** @param     参数    : double dou	无
        ** @return    返回    : private string	无
        ** @author    作者    : ysq
        ** @date      创建日期: 2022/7/8
        ** ----------------------------------------------------------------------------
        ** @change by 修改者  :
        ** @date      修改日期:
        ** @brier     修改内容:
        *******************************************************************************/
        private string HandleScientific(double dou)
        {
            string str = dou.ToString();
            string start = str.Substring(0, 5);
            int e = str.LastIndexOf('E');
            //MessageBox.Show(e.ToString());
            string end = str.Substring(e);
            string resultStr = start + end;
            return resultStr;
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
                //大于等于1的数的处理方法
                return HandleNumber(dou);
            }
            else if (dou >= 1e-4)
            {
                //小数的处理方法
                return HandleDecimal(dou);
            }
            else  // 小于0.0001会自动转为科学计数法，需要特殊处理
            {
                return HandleScientific(dou);
            }
        }

        /*******************************************************************************
        ** @name      函数名  : DouToSign
        ** @brief     功能    : 数字转为数字+量级符号，支持p,n,u,m,k,M
        ** @param     参数    : double dou	无
        ** @return    返回    : private string	无
        ** @author    作者    : ysq
        ** @date      创建日期: 2022/7/8
        ** ----------------------------------------------------------------------------
        ** @change by 修改者  :
        ** @date      修改日期:
        ** @brier     修改内容:
        *******************************************************************************/
        private string DouToSign(double dou)
        {
            string str;
            if (dou >= 1e6)
            {
                dou /= 1e6;
                str = HandleNumber(dou);
                str += 'M';
                return str;
            }
            else if (dou >= 1e3)
            {
                dou /= 1e3;
                str = HandleNumber(dou);
                str += 'k';
                return str;
            }
            else if (dou >= 1)
            {
                str = HandleNumber(dou);
                return str;
            }
            else if (dou >= 1e-3)
            {
                dou *= 1e3;
                str = HandleNumber(dou);
                str += 'm';
                return str;
            }
            else if (dou >= 1e-6)
            {
                dou *= 1e6;
                str = HandleNumber(dou);
                str += 'u';
                return str;
            }
            else if (dou >= 1e-9)
            {
                dou *= 1e9;
                str = HandleNumber(dou);
                str += 'n';
                return str;
            }
            else if (dou >= 1e-12)
            {
                dou *= 1e12;
                str = HandleNumber(dou);
                str += 'p';
                return str;
            }
            else
            {
                str = HandleScientific(dou);
                return str;
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
                    lc1 = Math.Pow(1 / (2 * f1 * Math.PI), 2);
                    LC.Text = DouToStr(lc1);
                }
            }
        }

        private void L_KeyDown(object sender, KeyEventArgs e)
        {
            if (LC.Text.Length != 0 && L.Text != "")
            {
                //取得L1的值
                l1 = StrToDou(L.Text);
                double result = lc1 / l1;
                //保存C1的值
                c1 = result;
                //C1的值渲染到页面上
                //C.Text = DouToStr(result);
                C.Text = DouToSign(result);
            }
            else
            {
                C.Text = "";
            }
        }
        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            if (LC.Text.Length != 0 && C.Text != "")
            {
                //double lcValue = LC1;
                //取得C1的值
                c1 = StrToDou(C.Text);
                //计算L1的值
                l1 = lc1 / c1;
                //把L的值渲染到页面上
                //L.Text = DouToStr(l1);
                L.Text = DouToSign(l1);
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
                XL.Text = DouToSign(xl);
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
                XC.Text = DouToSign(xc);
            }
            else
            {
                xc = 0;
                XC.Text = "";
            }
        }
    }
}
