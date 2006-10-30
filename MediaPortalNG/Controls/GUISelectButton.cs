using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MediaPortal
{

  public class GUISelectButton : Button, IGUIControl
  {
    private TextAlignment _align;
    private string _label;
    private string _texture;
    private string _hyperLink;
    private int _onUp;
    private int _onDown;
    private int _onLeft;
    private int _onRight;
    private static RoutedCommand _nextCommand;
    private static RoutedCommand _previousCommand;
    private static RoutedCommand _OpenCloseSelect;
    private int _controlId;

    private System.Collections.ArrayList _items;
    private int _selectedItem;
    private static bool _selectOpen;
    private static int _selectionFieldWidth;

    public GUISelectButton()
    {
      string styleName = this.GetType().ToString() + "Style";
      styleName = styleName.Replace("MediaPortal.", "");
      object resource = null;
      try
      {
        resource = this.FindResource(styleName);
        if (resource != null)
        {
          this.Style = resource as Style;
        }
      }
      catch { }
      _items = new System.Collections.ArrayList();
      _selectedItem = -1;
      InitializeCommands();
    }


    public int SelectionFieldWidth
    {
      get
      {
        return _selectionFieldWidth;
      }
      set
      {
        _selectionFieldWidth = value;
      }
    }

    public void AddItem(object item)
    {
      _items.Add(item);
    }

    public void DeleteItem(object item)
    {
      _items.Remove(item);
    }

    public void ClearItems()
    {
      _items.Clear();
    }

    protected override void OnClick()
    {
      base.OnClick();
      Core.OnClick(this);
    }

    public object SelectedItem
    {
      get
      {
        return (object)GetValue(SelectedItemProperty);
      }
      set { SetValue(SelectedItemProperty, value); }
    }



    public static readonly DependencyProperty SelectedItemProperty =DependencyProperty.Register("SelectedItem", typeof(object), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged)));


    private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;

      RoutedPropertyChangedEventArgs<object> e = new RoutedPropertyChangedEventArgs<object>((object)args.OldValue, (object)args.NewValue, SelectedItemChangedEvent);
      control.OnSelectedItemChanged(e);
    }


    protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> args)
    {
      RaiseEvent(args);
    }

    public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble,typeof(RoutedPropertyChangedEventHandler<object>), typeof(GUISelectButton));

    public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
    {
      add { AddHandler(SelectedItemChangedEvent, value); }
      remove { RemoveHandler(SelectedItemChangedEvent, value); }
    }

    private void UpdateValue()
    {
      if (_items != null)
      {
        if (_selectedItem < 0 || _selectedItem > _items.Count)
          return;
        SelectedItem = _items[_selectedItem];
      }
    }

    public static RoutedCommand OpenCloseSelect
    {
      get
      {
        return _OpenCloseSelect;
      }
    }


    public static RoutedCommand NextCommand
    {
      get
      {
        return _nextCommand;
      }
    }
    public static RoutedCommand PreviousCommand
    {
      get
      {
        return _previousCommand;
      }
    }

    private static void InitializeCommands()
    {
      _nextCommand = new RoutedCommand("NextCommand", typeof(GUISelectButton));
      CommandManager.RegisterClassCommandBinding(typeof(GUISelectButton), new CommandBinding(_nextCommand, OnNextCommand));
      CommandManager.RegisterClassInputBinding(typeof(GUISelectButton), new InputBinding(_nextCommand, new KeyGesture(Key.Right)));

      _previousCommand = new RoutedCommand("PreviousCommand", typeof(GUISelectButton));
      CommandManager.RegisterClassCommandBinding(typeof(GUISelectButton), new CommandBinding(_previousCommand, OnPreviousCommand));
      CommandManager.RegisterClassInputBinding(typeof(GUISelectButton), new InputBinding(_previousCommand, new KeyGesture(Key.Left)));

      _OpenCloseSelect = new RoutedCommand("OpenCloseSelect", typeof(GUISelectButton));
      CommandManager.RegisterClassCommandBinding(typeof(GUISelectButton), new CommandBinding(_OpenCloseSelect, OnOpenCloseSelect));
      CommandManager.RegisterClassInputBinding(typeof(GUISelectButton), new InputBinding(_OpenCloseSelect, new KeyGesture(Key.Return)));

    }

    private static void OnOpenCloseSelect(object sender, ExecutedRoutedEventArgs e)
    {
      GUISelectButton control = sender as GUISelectButton;
      _selectionFieldWidth = ((int)control.Width) - 50;

      if (control != null)
      {
        ControlTemplate t = control.Template;
        ControlTemplate rtp = (ControlTemplate)control.FindResource("SelectOpen");
        ControlTemplate rtr = (ControlTemplate)control.FindResource("SelectClose");

        if (rtr == null || rtp == null || t == null)
          return;

        if (_selectOpen == true)
        {
          control.Template = rtr;
          control.ApplyTemplate();
          _selectOpen = false;
          return;
        }
        else
        {
          control.Template = rtp;
          control.ApplyTemplate();
          _selectOpen = true;
          return;
        }

      }
    }

    private static void OnNextCommand(object sender, ExecutedRoutedEventArgs e)
    {
      GUISelectButton control = sender as GUISelectButton;
      if (control != null)
      {
        control.OnNext();
      }
    }

    private static void OnPreviousCommand(object sender, ExecutedRoutedEventArgs e)
    {
      GUISelectButton control = sender as GUISelectButton;
      if (control != null)
      {
        control.OnPrevious();
      }
    }

    protected virtual void OnNext()
    {
      if (_items == null) return;

      if (_selectedItem < _items.Count - 1)
      {
        _selectedItem += 1;
        UpdateValue();
      }
    }
    protected virtual void OnPrevious()
    {
      if (_items == null) return;

      if (_selectedItem > 0)
      {
        _selectedItem -= 1;
        UpdateValue();
      }
    }
    // control id
    public int ID
    {
      get
      {
        return _controlId;
      }
      set
      {
        _controlId = value;
      }
    }

    public static readonly DependencyProperty IDProperty =DependencyProperty.Register("ID", typeof(int), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIDChanged)));

    private static void OnIDChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;

      RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>((int)args.OldValue, (int)args.NewValue, IDChangedEvent);
      control.OnIDChanged(e);
    }

    public static readonly RoutedEvent IDChangedEvent = EventManager.RegisterRoutedEvent("IDChanged", RoutingStrategy.Bubble,typeof(RoutedPropertyChangedEventHandler<int>), typeof(GUISelectButton));

    public event RoutedPropertyChangedEventHandler<int> IDChanged
    {
      add { AddHandler(IDChangedEvent, value); }
      remove { RemoveHandler(IDChangedEvent, value); }
    }

    protected virtual void OnIDChanged(RoutedPropertyChangedEventArgs<int> args)
    {
      _controlId = args.NewValue;
      RaiseEvent(args);
    }

    public int OnUp
    {
      get
      {
        return _onUp;
      }
      set
      {
        _onUp = value;
      }
    }

    public static readonly DependencyProperty OnUpProperty =DependencyProperty.Register("OnUp", typeof(int), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnOnUpChanged)));

    private static void OnOnUpChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnOnUpChanged(args);
    }


    protected virtual void OnOnUpChanged(DependencyPropertyChangedEventArgs args)
    {
      _onUp = (int)args.NewValue;
    }


    // OnDown

    public int OnDown
    {
      get
      {
        return _onDown;
      }
      set
      {
        _onDown = value;
      }
    }

    public static readonly DependencyProperty OnDownProperty =DependencyProperty.Register("OnDown", typeof(int), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnOnDownChanged)));

    private static void OnOnDownChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnOnDownChanged(args);
    }


    protected virtual void OnOnDownChanged(DependencyPropertyChangedEventArgs args)
    {
      _onDown = (int)args.NewValue;
    }

    // OnLeft

    public int OnLeft
    {
      get
      {
        return _onLeft;
      }
      set
      {
        _onLeft = value;
      }
    }

    public static readonly DependencyProperty OnLeftProperty =DependencyProperty.Register("OnLeft", typeof(int), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnOnLeftChanged)));

    private static void OnOnLeftChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnOnLeftChanged(args);
    }


    protected virtual void OnOnLeftChanged(DependencyPropertyChangedEventArgs args)
    {
      _onLeft = (int)args.NewValue;
    }

    // OnRight

    public int OnRight
    {
      get
      {
        return _onRight;
      }
      set
      {
        _onRight = value;
      }
    }

    public static readonly DependencyProperty OnRightProperty =DependencyProperty.Register("OnRight", typeof(int), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnOnRightChanged)));

    private static void OnOnRightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnOnRightChanged(args);
    }


    protected virtual void OnOnRightChanged(DependencyPropertyChangedEventArgs args)
    {
      _onRight = (int)args.NewValue;
    }

    public string Label
    {
      get
      {
        return _label;
      }
      set
      {
        _label = value;
      }
    }

    public static readonly DependencyProperty LabelProperty =DependencyProperty.Register("Label", typeof(string), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnLabelChanged)));

    private static void OnLabelChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnLabelChanged(args);
    }

    protected virtual void OnLabelChanged(DependencyPropertyChangedEventArgs args)
    {
      _label = (string)args.NewValue;
      Content = Core.GetLocalizedString((string)args.NewValue);

    }

    // hyperlink
    public string Hyperlink
    {
      get
      {
        return _hyperLink;
      }
      set
      {
        _hyperLink = value;
      }
    }

    public static readonly DependencyProperty HyperlinkProperty =DependencyProperty.Register("Hyperlink", typeof(string), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnHyperlinkChanged)));

    private static void OnHyperlinkChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnHyperlinkChanged(args);
    }

    protected virtual void OnHyperlinkChanged(DependencyPropertyChangedEventArgs args)
    {
      _hyperLink = (string)args.NewValue;
    }

    // Texture
    public string Texture
    {
      get
      {
        return _texture;
      }
      set
      {
        _texture = value;
      }
    }

    public static readonly DependencyProperty TextureProperty =DependencyProperty.Register("Texture", typeof(string), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTextureChanged)));

    private static void OnTextureChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnTextureChanged(args);
    }

    protected virtual void OnTextureChanged(DependencyPropertyChangedEventArgs args)
    {
      _texture = (string)args.NewValue;
    }

    public TextAlignment Align
    {
      get
      {
        return _align;
      }
      set
      {
        _align = value;
      }
    }

    public static readonly DependencyProperty AlignProperty =DependencyProperty.Register("Align", typeof(TextAlignment), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnAlignChanged)));

    private static void OnAlignChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnAlignChanged(args);
    }

    protected virtual void OnAlignChanged(DependencyPropertyChangedEventArgs args)
    {
      _align = (TextAlignment)args.NewValue;
    }
    // disabled color
    private Brush _disabledColor;

    public Brush DisabledColor
    {
      get
      {
        return _disabledColor;
      }
      set
      {
        _disabledColor = value;
      }
    }

    public static readonly DependencyProperty DisabledColorProperty =DependencyProperty.Register("DisabledColor", typeof(Brush), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDisabledColorChanged)));

    private static void OnDisabledColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnDisabledColorChanged(args);
    }

    protected virtual void OnDisabledColorChanged(DependencyPropertyChangedEventArgs args)
    {
      _disabledColor = (Brush)args.NewValue;
    }


    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
      base.OnRenderSizeChanged(sizeInfo);
      if (_align == TextAlignment.Right)
        Canvas.SetLeft(this, _positionX - sizeInfo.NewSize.Width);
    }
    string IGUIControl.Label
    {
      get
      {
        return (string)GetValue(LabelProperty);
      }
      set
      {
        SetValue(LabelProperty, value);
      }
    }


    int IGUIControl.ID
    {
      get { return _controlId; }
    }

    int IGUIControl.OnUp
    {
      get { return _onUp; }
    }

    int IGUIControl.OnDown
    {
      get { return _onDown; }
    }

    int IGUIControl.OnLeft
    {
      get { return _onLeft; }
    }

    int IGUIControl.OnRight
    {
      get { return _onRight; }
    }


    //
    // property PosY
    // 
    private double _positionY;

    public double PosY
    {
      get
      {
        return (double)GetValue(PosYProperty);
      }
      set
      {
        SetValue(PosYProperty, value);
      }
    }

    public static readonly DependencyProperty PosYProperty =DependencyProperty.Register("PosY", typeof(double), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPosYChanged)));

    private static void OnPosYChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnPosYChanged(args);
    }

    protected virtual void OnPosYChanged(DependencyPropertyChangedEventArgs args)
    {
      _positionY = (double)args.NewValue;
      Canvas.SetTop(this, _positionY);
    }

    //
    // property PosX
    // 
    private double _positionX;

    public double PosX
    {
      get
      {
        return (double)GetValue(PosXProperty);
      }
      set
      {
        SetValue(PosXProperty, value);
      }
    }

    public static readonly DependencyProperty PosXProperty =DependencyProperty.Register("PosX", typeof(double), typeof(GUISelectButton),new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPosXChanged)));

    private static void OnPosXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      GUISelectButton control = (GUISelectButton)obj;
      control.OnPosXChanged(args);
    }

    protected virtual void OnPosXChanged(DependencyPropertyChangedEventArgs args)
    {
      _positionX = (double)args.NewValue;
      Canvas.SetLeft(this, _positionX);
    }
  }
}

