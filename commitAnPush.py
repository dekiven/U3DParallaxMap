# -*-coding:utf-8-*-
import os
from TkToolsD.CommonWidgets import *
from DKVTools.Funcs import *
from TkToolsD.TextScrollView import *

curDir = os.getcwd()
logLabel = None

def __main() :
    # print(type(tryCmd))
    # print(type(centerToplevel))
    tk, ttk = getTk()

    app = tk.Tk()
    app.rowconfigure(0, weight=1)
    app.columnconfigure(0, weight=1)
    app.bind('<KeyPress-Escape>', lambda p : app.quit())

    frame = ttk.LabelFrame(app, text='enter commit msg!')
    # frame.rowconfigure(0, weight=1)
    # frame.rowconfigure(1, weight=1)
    frame.columnconfigure(0, weight=1)
    frame.grid(row=0, column=0, sticky='nswe')

    count = getCounter()

    label = ttk.Label(frame, text='msg:')
    label.grid(row=count(), column=0, sticky='w', padx=20)

    entry = GetEntry(frame)
    entry.grid(row=count(), column=0, sticky='nswe', padx=20)
    entry.focus_set()

    label = ttk.Label(frame, text='log:')
    label.grid(row=count(), column=0, sticky='w', padx=20)

    # sv = tk.StringVar()
    # label = tk.Label(frame, textvariable=sv, height=10, width=30, anchor='nw')
    label = TextScrollView(frame, height=100, width=30, undo=True)
    c = count()
    label.grid(row=c, column=0, sticky='nswe', padx=20)
    frame.rowconfigure(c, weight=1)
    def __set (s) :
        try:
            # 在没有修改的情况下撤销会有异常
            # 没有找到清空文字的方法，使用text自带的撤销功能撤销
            label.edit_undo()
        except Exception as e:
            print(e)
        label.insert(tk.END, s)
        print(s)

    label.set = __set
    global logLabel
    logLabel = label

    btn = ttk.Button(frame, text='commit', command=lambda : __onCommit(entry.get()))
    btn.grid(row=count(), column=0, padx=20, pady=10)

    app.update()
    # centerToplevel(app)
    app.mainloop()

def __onCommit(msg) :
    # print('commit:%s'%(msg))
    if msg.strip() == '' :
        ShowInfoDialog('input msg!')
        return

    os.chdir(curDir)
    rst = tryCmd('''git add *.*
git commit -m "%s"
git push
'''%(msg))
    
    log = ''
    for l in rst :
        log += l
    logLabel.set(log)
    

if __name__ == '__main__' :
    __main()
    # import TkToolsD
    # from TkToolsD.TextScrollView import *
    # help(TextScrollView)
