using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Animation;
using Android.Util;
using System.Threading;
using Java.Lang;
using System.Threading.Tasks;


namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.activity_main);

            View colorBlock = FindViewById<View>(Resource.Id.colorBlock);
            ShowBlock(colorBlock);

        }

        private int GetScreenSize(char target)
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);

            int screenWidthPixels = displayMetrics.WidthPixels; // 屏幕宽度（像素）
            int screenHeightPixels = displayMetrics.HeightPixels; // 屏幕高度（像素）


            if (target == 'h')
            {
                return screenHeightPixels;
            }
            else if(target == 'w')
            {
                return screenWidthPixels;
            }
            return 0;
        }

        private int GetCurrentSeconds()
        {
            DateTime currentDateTime = DateTime.Now;
            int currentSeconds = currentDateTime.Second;

            // 打印当前秒数
            Log.Info("CurrentTime", $"Current Seconds: {currentSeconds}");
            return currentSeconds;
        }

        private async void ShowBlock(View colorBlock)
        {
            while (true)
            {
                int nowTime = GetCurrentSeconds();
                int screenSize = GetScreenSize('h');

                int nowSize = nowTime * screenSize / 60;
                int targetSize = screenSize;

                // 创建两个 ValueAnimator 对象，一个用于动画的起始值，一个用于动画的终止值
                ValueAnimator startAnimator = ValueAnimator.OfInt(nowSize, targetSize);
                ValueAnimator endAnimator = ValueAnimator.OfInt(targetSize, 0); // 将结束时的高度设置为 0

                // 设置动画持续时间
                int duration = (60 - nowTime) * 1000;
                startAnimator.SetDuration(duration);
                endAnimator.SetDuration(1);

                // 创建 AnimatorSet 对象，用于组合动画
                AnimatorSet animatorSet = new AnimatorSet();

                // 将两个动画添加到 AnimatorSet 中
                animatorSet.PlaySequentially(startAnimator, endAnimator);

                // 在动画更新时修改颜色块的大小
                startAnimator.Update += (sender, e) =>
                {
                    int animatedValue = (int)e.Animation.AnimatedValue;

                    // 设置新的高度
                    colorBlock.LayoutParameters.Height = animatedValue;

                    // 请求布局以更新视图
                    colorBlock.RequestLayout();
                };

                // 在动画更新时修改颜色块的大小
                endAnimator.Update += (sender, e) =>
                {
                    int animatedValue = (int)e.Animation.AnimatedValue;

                    // 设置新的高度
                    colorBlock.LayoutParameters.Height = animatedValue;

                    // 请求布局以更新视图
                    colorBlock.RequestLayout();
                };

                // 启动动画
                animatorSet.Start();

                // 等待动画结束
                await Task.Delay(duration + 1);
            }
        }




    }
}
