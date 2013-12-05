using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImageSDK
{
    /// <summary>
    /// ����ͼ��ӿ�
    /// </summary>
	public interface IImage:IDisposable
	{
        /// <summary>
        /// ����
        /// </summary>e
        void Draw(IntPtr handle, int width, int height);

        void Draw(IntPtr handle, Rectangle destRect, int width, int height);

        void Draw(Graphics g ,int left, int top, int width, int height);
        /// <summary>
        /// ���浽�ļ�
        /// </summary>
        void SaveToFile(string fileaName);

        void SaveToStream(System.IO.Stream sm);

        void fromImage(IImage src);

        IntPtr getHBitmap();

        long addRef();
        long releaseRef();
    }
}
