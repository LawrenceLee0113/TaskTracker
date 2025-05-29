/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",
    "./Areas/**/*.cshtml",
    "./wwwroot/**/*.html"
  ],
  theme: {
    extend: {
      // 🎨 專業色彩系統 - 基於您的建議
      colors: {
        // 主色：藍色系 (穩重、專業、信任感)
        primary: {
          50: '#EBF5FF',
          100: '#E1EFFE', 
          200: '#C3DDFD',
          300: '#A4CAFE',
          400: '#76A9FA',
          500: '#2563EB', // 主色
          600: '#1D4ED8',
          700: '#1E40AF',
          800: '#1E3A8A',
          900: '#1E3A8A',
          950: '#172554'
        },
        // 輔助色：綠色系 (積極、完成、正向)
        success: {
          50: '#F0FDF4',
          100: '#DCFCE7',
          200: '#BBF7D0',
          300: '#86EFAC',
          400: '#4ADE80',
          500: '#22C55E', // 輔助色
          600: '#16A34A',
          700: '#15803D',
          800: '#166534',
          900: '#14532D'
        },
        // 警示色：橘色系 (提醒、警示)
        warning: {
          50: '#FFFBEB',
          100: '#FEF3C7',
          200: '#FDE68A',
          300: '#FCD34D',
          400: '#FBBF24',
          500: '#F59E42', // 警示色
          600: '#D97706',
          700: '#B45309',
          800: '#92400E',
          900: '#78350F'
        },
        // 錯誤色：紅色系 (錯誤、逾期)
        danger: {
          50: '#FEF2F2',
          100: '#FEE2E2',
          200: '#FECACA',
          300: '#FCA5A5',
          400: '#F87171',
          500: '#EF4444', // 錯誤色
          600: '#DC2626',
          700: '#B91C1C',
          800: '#991B1B',
          900: '#7F1D1D'
        },
        // 中性色系 (背景、文字、邊框)
        gray: {
          50: '#F9FAFB',   // 背景色
          100: '#F3F4F6',
          200: '#E5E7EB',  // 邊框/分隔
          300: '#D1D5DB',
          400: '#9CA3AF',
          500: '#6B7280',
          600: '#4B5563',
          700: '#374151',
          800: '#1F2937',
          900: '#111827',
          950: '#0F1419'   // 深色模式主色
        },
        // 專案狀態色彩
        status: {
          planning: '#8B5CF6',    // 規劃中 - 紫色
          active: '#2563EB',      // 進行中 - 藍色  
          paused: '#F59E42',      // 暫停 - 橘色
          completed: '#22C55E',   // 已完成 - 綠色
          cancelled: '#EF4444'    // 已取消 - 紅色
        }
      },
      // 🔤 字體系統
      fontFamily: {
        'sans': ['Inter', 'system-ui', 'sans-serif'],
        'mono': ['JetBrains Mono', 'Monaco', 'Consolas', 'monospace']
      },
      // 📏 間距系統
      spacing: {
        '18': '4.5rem',
        '88': '22rem',
        '128': '32rem'
      },
      // 🖼️ 陰影系統 (現代化)
      boxShadow: {
        'soft': '0 2px 15px -3px rgba(0, 0, 0, 0.07), 0 10px 20px -2px rgba(0, 0, 0, 0.04)',
        'medium': '0 4px 25px -5px rgba(0, 0, 0, 0.1), 0 20px 40px -7px rgba(0, 0, 0, 0.1)',
        'large': '0 8px 50px -12px rgba(0, 0, 0, 0.25)',
        'glow': '0 0 20px rgba(37, 99, 235, 0.4)'
      },
      // 🎯 邊框圓角 (現代感)
      borderRadius: {
        'xl': '0.75rem',
        '2xl': '1rem',
        '3xl': '1.5rem'
      },
      // ⚡ 動畫和過渡
      transitionProperty: {
        'height': 'height',
        'spacing': 'margin, padding'
      },
      animation: {
        'fade-in': 'fadeIn 0.5s ease-in-out',
        'slide-up': 'slideUp 0.3s ease-out',
        'bounce-light': 'bounce 1s infinite'
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' }
        },
        slideUp: {
          '0%': { transform: 'translateY(10px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' }
        }
      }
    },
  },
  plugins: [
    // 🔌 添加實用的插件
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography')
  ],
  // 🌙 深色模式支持 (未來擴展)
  darkMode: 'class'
} 